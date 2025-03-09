using CommunityHub.Core.Models;

namespace CommunityHub.Infrastructure.Services
{
    public interface IRegistrationService
    {
        Task<RegistrationRequest> CreateRequestAsync(RegistrationData registrationData);
    }
}