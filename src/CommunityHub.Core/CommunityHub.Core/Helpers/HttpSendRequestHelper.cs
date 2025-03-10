namespace CommunityHub.Core.Helpers
{
    public class HttpSendRequestHelper
    {
        public static async Task<T> SendGetRequest<T>(HttpClient client, string url)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            var response = await client.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"HTTP request failed with status code {response.StatusCode}. Error: {errorContent}");
            }

            return await HttpHelper.GetHttpResponseObject<T>(response);
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
                    throw new Exception($"HTTP request failed with status code {response.StatusCode}. Error: {errorContent}");
                }

                return await HttpHelper.GetHttpResponseObject<V>(response);
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
    }
}
