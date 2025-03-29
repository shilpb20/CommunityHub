using CommunityHub.Infrastructure.Models;

namespace CommunityHub.Infrastructure.Services.Account
{
    public interface IJwtTokenService
    {
        Task<string> GenerateTokenAsync(ApplicationUser user);
    }
}