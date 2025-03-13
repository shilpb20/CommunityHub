using Newtonsoft.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CommunityHub.Core.Helpers
{
    public class HttpSendRequestHelper
    {
        public static async Task<T> SendGetRequestAsync<T>(HttpClient client, string url)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, url);
                return await SendAndProcessResponse<T>(client, request);
            }
            catch (HttpRequestException httpEx)
            {
                throw new Exception("An error occurred while sending the GET request.", httpEx);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred in sending the GET request.");
            }
        }

        public static async Task<V> SendPostRequestAsync<T, V>(HttpClient client, string url, T? data)
        {
            try
            {
                HttpRequestMessage request = HttpHelper.GetHttpPostRequest<T>(url, data);
                return await SendAndProcessResponse<V>(client, request);
            }
            catch (HttpRequestException httpEx)
            {
                throw new Exception("An error occurred while sending the POST request.", httpEx);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred in sending the POST request.");
            }
        }

        public static async Task<T> SendUpdateRequestAsync<T>(HttpClient client, string url, int id, T? data)
        {
            return await SendUpdateRequestAsync<T, T>(client, url, id, data);
        }

        public static async Task<V> SendUpdateRequestAsync<T, V>(HttpClient client, string url, int id, T? data)
        {
            try
            {
                var Uri = new Uri(client.BaseAddress, url);
                HttpRequestMessage request = HttpHelper.GetHttpPutRequest<T>(Uri.ToString(), id, data);
                return await SendAndProcessResponse<V>(client, request);
            }
            catch (HttpRequestException httpEx)
            {
                throw new Exception("An error occurred while sending the PUT request.", httpEx);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred in sending the PUT request.");
            }
        }

        private static async Task<T> SendAndProcessResponse<T>(HttpClient client, HttpRequestMessage request)
        {
            var response = await client.SendAsync(request);
            return await ProcessResponse<T>(response);
        }

        private static async Task<V> ProcessResponse<V>(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"HTTP request failed with status code {response.StatusCode}. Error: {errorContent}");
            }

            return await HttpHelper.GetHttpResponseObject<V>(response);
        }
    }
}
