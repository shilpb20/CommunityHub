using AppComponents.Repository.Abstraction;
using CommunityHub.Core.Dtos;
using CommunityHub.Core.Enums;
using CommunityHub.Infrastructure.Models;
using System.Linq.Expressions;

namespace CommunityHub.Infrastructure.Services.User
{
    public interface IUserService
    {
        Task<UserInfo> GetUserAsyncById(int id);
        Task<List<UserInfo>> GetUsersAsync(string? sortBy, bool ascending);

        Task<UserInfo> CreateUserAsync(UserInfo userInfo);
        Task<UserInfo> GetUserInfoByContactNumber(string countryCode, string contactNumber);
        Task<UserInfo> GetUserInfoByEmail(string email);
    }
}