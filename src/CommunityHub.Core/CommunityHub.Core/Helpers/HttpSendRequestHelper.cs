using Newtonsoft.Json;

namespace CommunityHub.Core.Helpers
{
    public class HttpSendRequestHelper
    {
        public static async Task<T> SendGetRequestAsync<T>(HttpClient client, string url)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            var response = await client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"HTTP request failed with status code {response.StatusCode}. Error: {errorContent}");
            }

            try
            {
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<T>(content);
                return result;
            }
            catch (JsonException jsonEx)
            {
                throw new Exception($"Error deserializing the response content. {jsonEx.Message}");
            }
        }


        public static async Task<V> SendPostRequest<T, V>(HttpClient client, string url, T? data)
        {
            try
            {
                HttpRequestMessage request = HttpHelper.GetHttpPostRequest<T>(url, data);
                var response = await client.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    throw new HttpRequestException($"HTTP request failed with status code {response.StatusCode}. Error: {errorContent}");
                }

                return await HttpHelper.GetHttpResponseObject<V>(response);
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
    }
}
