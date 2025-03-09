
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using JsonException = Newtonsoft.Json.JsonException;

namespace CommunityHub.Tests.Core.Helpers
{
    public class HttpHelper
    {
        public static HttpRequestMessage GetHttpPostRequest<T>(string url, T data)
        {
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, url);
            if (data != null)
            {
                var json = JsonConvert.SerializeObject(data);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                requestMessage.Content = content;
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
