using CommunityHub.Core.Models;

namespace CommunityHub.UI.Services
{
    public interface IAccountService
    {
        Task<ApiResponse<LoginResponse>> LoginAsync(Login model);
        Task<ApiResponse<bool>> SetNewPasswordAsync(SetPassword model);
        Task<ApiResponse<bool>> SendPasswordResetEmailAsync(string email);
    }
}