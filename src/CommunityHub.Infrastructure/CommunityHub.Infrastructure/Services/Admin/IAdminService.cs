using CommunityHub.Core.Enums;
using CommunityHub.Infrastructure.Models;

namespace CommunityHub.Infrastructure.Services.AdminService
{
    public interface IAdminService
    {
        Task<RegistrationRequest> RejectRequestAsync(int id, string reviewComment);
        Task<UserInfo> ApproveRequestAsync(RegistrationRequest request);
    }
}