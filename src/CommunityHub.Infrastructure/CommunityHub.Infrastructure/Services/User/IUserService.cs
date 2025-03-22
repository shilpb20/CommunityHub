using CommunityHub.Infrastructure.Models;

namespace CommunityHub.Infrastructure.Services.User
{
    public interface IUserService
    {
        Task<UserInfo> CreateUserAsync(UserInfo userInfo,
            SpouseInfo? spouseInfo,
            List<Children>? children);

        Task<UserInfo> GetUserAsync(int id);
        Task<List<UserInfo>> GetUsersAsync(Dictionary<string, bool>? orderBy = null);
    }
}