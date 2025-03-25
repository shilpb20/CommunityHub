using CommunityHub.Infrastructure.Models;
using System.Linq.Expressions;

namespace CommunityHub.Infrastructure.Services.User
{
    public interface IUserService
    {
        Task<UserInfo> GetUserAsyncById(int id);
        Task<List<UserInfo>> GetUsersAsync(Dictionary<string, bool>? orderBy = null);

        //Task<UserInfo> CreateUserAsync(UserInfo userInfo,
        //    SpouseInfo? spouseInfo,
        //    List<Children>? children);

        //Task<UserInfo> GetUserAsync(Expression<Func<UserInfo, bool>> filter);
        //Task<UserInfo> UpdateUsersAsync(UserInfo userInfo);
        //Task<UserInfo> DeleteUsersAsync(int id);
    }
}