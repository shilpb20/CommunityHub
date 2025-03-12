using CommunityHub.Core.Enums;
using CommunityHub.Core.Models;

namespace CommunityHub.Infrastructure.Services
{
    public interface IRegistrationService
    {
        Task<RegistrationRequest> CreateRequestAsync(RegistrationData registrationData);
        Task<RegistrationRequest> GetRequestAsync(int id);
        Task<List<RegistrationRequest>> GetRequestsAsync(RegistrationStatus status);
    }
}