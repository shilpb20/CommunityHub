using CommunityHub.Infrastructure.Models;
using System.Linq.Expressions;

namespace CommunityHub.Infrastructure.Services.User
{
    public interface IUserService
    {
        Task<UserInfo> CreateUserAsync(UserInfo userInfo,
            SpouseInfo? spouseInfo,
            List<Children>? children);

        Task<UserInfo> GetUserAsync(int id);
        Task<UserInfo> GetUserAsync(Expression<Func<UserInfo, bool>> filter);
        Task<List<UserInfo>> GetUsersAsync(Dictionary<string, bool>? orderBy = null);

        Task<UserInfo> UpdateUsersAsync(UserInfo userInfo);
        Task<UserInfo> DeleteUsersAsync(int id);
    }
}