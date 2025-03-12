using CommunityHub.Core.Models;

namespace CommunityHub.Api.Services
{
    public interface IRegistrationService
    {
        Task<RegistrationRequest> CreateRequestAsync(RegistrationData registrationData);
    }
}