using CommunityHub.Core.Enums;
using CommunityHub.Infrastructure.Models;

namespace CommunityHub.Infrastructure.Services.Registration
{
    public interface IRegistrationService
    {
        Task<RegistrationRequest> CreateRequestAsync(RegistrationInfo registrationData);
        Task<RegistrationRequest> GetRequestByIdAsync(int id);
        Task<List<RegistrationRequest>> GetRequestsAsync(eRegistrationStatus status = eRegistrationStatus.Pending);
        Task<RegistrationRequest> UpdateRequestAsync(RegistrationRequest registrationRequest);
    }
}
