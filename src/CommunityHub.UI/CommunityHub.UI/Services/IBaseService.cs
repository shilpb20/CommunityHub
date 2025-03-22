
namespace CommunityHub.UI.Services
{
    public interface IBaseService
    {
        public HttpClient GetClient();

        Task<V> AddRequestAsync<T, V>(string url, int id, T? data);
        Task<V> AddRequestAsync<T, V>(string url, T? data);

        Task<T> GetRequestAsync<T>(string url);
        Task<T> GetRequestAsync<T>(string url, int id);

        Task<V> UpdateRequestAsync<T, V>(string url, int id, T? data);
    }
}