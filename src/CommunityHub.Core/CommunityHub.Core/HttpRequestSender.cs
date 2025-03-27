using CommunityHub.Core.Helpers;
using CommunityHub.Core.Models;
using CommunityHub.Core;
using Newtonsoft.Json;
using CommunityHub.Core.Messages;
using CommunityHub.Core.Factory;

public class HttpRequestSender : IHttpRequestSender
{
    private readonly IResponseFactory _responseFactory;

    public HttpRequestSender(IResponseFactory responseFactory)
    {
        _responseFactory = responseFactory;
    }

    public async Task<ApiResponse<T>> SendGetRequestAsync<T>(HttpClient client, string url)
    {
        try
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            return await SendAndProcessResponse<T>(client, request);
        }
        catch (HttpRequestException httpEx)
        {
            return _responseFactory.Failure<T>(ErrorCode.RequestError, "An error occurred while sending the GET request.");
        }
        catch
        {
            return _responseFactory.Failure<T>(ErrorCode.UnknownError, "An unexpected error occurred in sending the GET request.");
        }
    }

    public async Task<ApiResponse<V>> SendPostRequestAsync<T, V>(HttpClient client, string url, T? data)
    {
        try
        {
            HttpRequestMessage request = HttpHelper.GetHttpPostRequest<T>(url, data);
            return await SendAndProcessResponse<V>(client, request);
        }
        catch (HttpRequestException httpEx)
        {
            return _responseFactory.Failure<V>(ErrorCode.RequestError, "An error occurred while sending the POST request.");
        }
        catch
        {
            return _responseFactory.Failure<V>(ErrorCode.UnknownError, "An unexpected error occurred in sending the POST request.");
        }
    }

    public async Task<ApiResponse<T>> SendUpdateRequestAsync<T>(HttpClient client, string url, int id, T? data)
    {
        return await SendUpdateRequestAsync<T, T>(client, url, id, data);
    }

    public async Task<ApiResponse<V>> SendUpdateRequestAsync<T, V>(HttpClient client, string url, int id, T? data)
    {
        try
        {
            string formattedUrl = ApiRouteHelper.FormatRoute(url, id);
            var Uri = new Uri(client.BaseAddress, formattedUrl);
            HttpRequestMessage request = HttpHelper.GetHttpPutRequest<T>(Uri.ToString(), id, data);
            return await SendAndProcessResponse<V>(client, request);
        }
        catch (HttpRequestException httpEx)
        {
            return _responseFactory.Failure<V>(ErrorCode.RequestError, "An error occurred while sending the PUT request.");
        }
        catch
        {
            return _responseFactory.Failure<V>(ErrorCode.UnknownError, "An unexpected error occurred in sending the PUT request.");
        }
    }

    private async Task<ApiResponse<T>> SendAndProcessResponse<T>(HttpClient client, HttpRequestMessage request)
    {
        var response = await client.SendAsync(request);
        return await ProcessResponse<T>(response);
    }

    private async Task<ApiResponse<T>> ProcessResponse<T>(HttpResponseMessage response)
    {
        if (!response.IsSuccessStatusCode)
        {
            string errorContent = await response.Content.ReadAsStringAsync();

            string errorCode;
            string errorMessage;
            try
            {
                var errorObj = JsonConvert.DeserializeObject<ErrorResponse>(errorContent);

                errorCode = errorObj.ErrorCode ?? ErrorCode.HttpError;
                errorMessage = errorObj.ErrorMessage ?? errorContent;
            }
            catch
            {
                errorCode = ErrorCode.HttpError;
                errorMessage = errorContent;
            }

            return _responseFactory.Failure<T>(errorCode, errorMessage);
        }

        try
        {
            var data = await response.Content.ReadAsStringAsync();
            var parsedData = JsonConvert.DeserializeObject<ApiResponse<T>>(data);
            return _responseFactory.Success(parsedData.Data);
        }
        catch (Exception ex)
        {
            return _responseFactory.Failure<T>(ErrorCode.ResponseParseError, $"An error occurred while processing the response: {ex.Message}");
        }
    }
}
