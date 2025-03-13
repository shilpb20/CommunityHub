
using Newtonsoft.Json;
using System.Text;
using JsonException = Newtonsoft.Json.JsonException;

namespace CommunityHub.Core.Helpers
{
    public class HttpHelper
    {
        public static HttpRequestMessage GetHttpGetRequestById(string url, int id)
        {
            var formattedUrl = url.TrimEnd('/') + "/" + id;
            return GetHttpGetRequest(formattedUrl);
        }

        public static HttpRequestMessage GetHttpGetRequest(string url)
        {
            return new HttpRequestMessage(HttpMethod.Get, url);
        }

        public static HttpRequestMessage GetHttpPutRequest<T>(string url, int id, T? data)
        {
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Put, url + $"/{id}");
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

        public static HttpRequestMessage GetHttpPutRequest(string url)
        {
            return new HttpRequestMessage(HttpMethod.Put, url);
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
