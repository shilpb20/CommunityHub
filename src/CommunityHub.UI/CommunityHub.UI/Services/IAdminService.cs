using CommunityHub.Core.Dtos;
using CommunityHub.Core.Models;

namespace CommunityHub.UI.Services
{
    public interface IAdminService
    {
        Task<ApiResponse<List<RegistrationRequestDto>>> GetPendingRequests();
        Task<ApiResponse<RegistrationRequestDto>> RejectRegistrationRequest(int id, string rejectionReason);
    }
}