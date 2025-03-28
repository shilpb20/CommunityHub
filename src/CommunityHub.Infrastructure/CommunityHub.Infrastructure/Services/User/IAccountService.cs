using AppComponents.Repository.Abstraction;
using CommunityHub.Infrastructure.Models;
using Microsoft.AspNetCore.Identity;

namespace CommunityHub.Infrastructure.Services.User
{
    public interface IAccountService
    {
        public Task<ApplicationUser> CreateAccountAsync(UserInfo userInfo);
        Task<string> GenerateTokenAsync(ApplicationUser applicationUser);
    }
}
