
using Newtonsoft.Json;
using System.Text;
using JsonException = Newtonsoft.Json.JsonException;

namespace CommunityHub.Tests.Core.Helpers
{
    public class HttpHelper
    {
        public static HttpRequestMessage GetHttpGetRequestById(string url, int id)
        {
            var formattedUrl = url.TrimEnd('/') + "/" + id;
            return new HttpRequestMessage(HttpMethod.Get, formattedUrl);
        }

        public static async Task<V> SendHttpPostRequest<T, V>(HttpClient client, string url, T? data)
        {
            try
            {
                HttpRequestMessage request = GetHttpPostRequest<T>(url, data);
                var response = await client.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    throw new Exception($"HTTP request failed with status code {response.StatusCode}. Error: {errorContent}");
                }

                return await GetHttpResponseObject<V>(response);
            }
            catch (HttpRequestException httpEx)
            {
                throw new Exception("Network error occurred while sending the POST request.", httpEx);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred in sending the POST request.");
            }
        }


        public static HttpRequestMessage GetHttpPostRequest<T>(string url, T? data)
        {
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, url);

            var json = data != null ? JsonConvert.SerializeObject(data) : null;
            if (json != null)
            {
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                requestMessage.Content = content;
            }
            else
            {
                requestMessage.Content = null;
            }

            return requestMessage;
        }



        public static async Task<T?> GetHttpResponseObject<T>(HttpResponseMessage response)
        {
            var responseString = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrWhiteSpace(responseString))
            {
                return default;
            }

            try
            {
                if (!response.IsSuccessStatusCode)
                {
                    return default;
                }

                return JsonConvert.DeserializeObject<T>(responseString);
            }
            catch (JsonException)
            {
                return default;
            }
        }
    }
}
