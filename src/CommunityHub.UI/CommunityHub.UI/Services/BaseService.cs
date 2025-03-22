using CommunityHub.Core.Helpers;
using Microsoft.Extensions.Options;

namespace CommunityHub.UI.Services
{
    public class BaseService : IBaseService
    {
        private readonly ApiSettings _apiSettings;
        private readonly HttpClient _httpClient;

        public BaseService(HttpClient httpClient, IOptions<ApiSettings> apiSettings)
        {
            _apiSettings = apiSettings.Value;

            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(_apiSettings.BaseUrl);
        }

        public async Task<T> GetRequestAsync<T>(string url)
        {
            return await HttpSendRequestHelper.SendGetRequestAsync<T>(_httpClient, url);
        }

        public async Task<T> GetRequestAsync<T>(string url, int id)
        {
            string formattedUrl = ApiRouteHelper.FormatRoute(url, id);
            return await HttpSendRequestHelper.SendGetRequestAsync<T>(_httpClient, formattedUrl);
        }

        public async Task<V> AddRequestAsync<T, V>(string url, T? data)
        {
            return await HttpSendRequestHelper.SendPostRequestAsync<T, V>(_httpClient, url, data);
        }

        public async Task<V> AddRequestAsync<T, V>(string url, int id, T? data)
        {
            string formattedUrl = ApiRouteHelper.FormatRoute(url, id);
            return await HttpSendRequestHelper.SendPostRequestAsync<T, V>(_httpClient, formattedUrl, data);
        }

        public async Task<V> UpdateRequestAsync<T, V>(string url, int id, T? data)
        {
            return await HttpSendRequestHelper.SendUpdateRequestAsync<T, V>(_httpClient, url, id, data);
        }

        public HttpClient GetClient()
        {
            return _httpClient;
        }
    }
}
