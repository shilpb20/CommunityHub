using CommunityHub.Core.Enums;
using CommunityHub.Core.Models;

namespace CommunityHub.Infrastructure.Services.Registration
{
    public interface IRegistrationService
    {
        Task<RegistrationRequest> CreateRequestAsync(RegistrationInfo registrationData);
        Task<RegistrationRequest> GetRequestAsync(int id);
        Task<List<RegistrationRequest>> GetRequestsAsync(RegistrationStatus status);
        Task<RegistrationRequest> RejectRequestAsync(int id, string reviewComment);
        Task<UserInfo> ApproveRequestAsync(int id);
    }
}