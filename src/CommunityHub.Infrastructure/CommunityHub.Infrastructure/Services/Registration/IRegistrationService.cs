using CommunityHub.Core.Enums;
using CommunityHub.Infrastructure.Models;

namespace CommunityHub.Infrastructure.Services.Registration
{
    public interface IRegistrationService
    {
        Task<RegistrationRequest> CreateRequestAsync(RegistrationInfo registrationData);
        Task<RegistrationRequest> ApproveRequestAsync(int id);
        Task<RegistrationRequest> RejectRequestAsync(int id, string comment);

        Task<RegistrationRequest> GetRequestByIdAsync(int id);
        Task<List<RegistrationRequest>> GetRequestsAsync(eRegistrationStatus status);
    }
}
