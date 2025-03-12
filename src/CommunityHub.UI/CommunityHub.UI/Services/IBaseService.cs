
namespace CommunityHub.UI.Services
{
    public interface IBaseService
    {
        public HttpClient GetClient();

        Task<V> AddRequestAsync<T, V>(string url, T? data);
        Task<T> GetRequestAsync<T>(string url);
    }
}