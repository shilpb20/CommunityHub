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

        public async Task<V> AddRequestAsync<T, V>(string url, T? data)
        {
            return await HttpSendRequestHelper.SendPostRequest<T, V>(_httpClient, url, data);
        }
    }
}
