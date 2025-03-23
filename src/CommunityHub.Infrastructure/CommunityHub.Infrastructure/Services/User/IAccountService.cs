using CommunityHub.Infrastructure.Models;
using Microsoft.AspNetCore.Identity;

namespace CommunityHub.Infrastructure.Services.User
{
    public interface IAccountService
    {
        public Task<IdentityResult> CreateUserAsync(ApplicationUser user);
        public Task<IdentityResult> DeleteAccountAsync(ApplicationUser user);
    }
}
