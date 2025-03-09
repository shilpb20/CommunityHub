using AppComponents.Repository.Abstraction;
using CommunityHub.Core.Models;
using CommunityHub.Infrastructure.Data;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;

namespace CommunityHub.Infrastructure.Services
{
    public class RegistrationService : IRegistrationService
    {
        private readonly ILogger<IRegistrationService> _logger;
        private readonly IRepository<RegistrationRequest, ApplicationDbContext> _repository;

        public RegistrationService(
            ILogger<IRegistrationService> logger,
            IRepository<RegistrationRequest, ApplicationDbContext> repository)
        {
            _logger = logger;
            _repository = repository;
        }

        public async Task<RegistrationRequest> CreateRequestAsync(RegistrationData registrationData)
        {
            try
            {
                if (registrationData == null)
                {
                    _logger.LogDebug("Registration data null. Skipping request creation");
                    return null;
                }

                _logger.LogDebug($"Creating registration request for user - {registrationData?.UserDetails.FullName}.");
                RegistrationRequest registrationRequest = new RegistrationRequest()
                {
                    RegistrationData = JsonConvert.SerializeObject(registrationData),
                    CreatedAt = DateTime.UtcNow,
                    RegistrationStatus = "Pending",
                    Review = null,
                    ReviewedAt = null
                };

                return await _repository.AddAsync(registrationRequest);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while creating registration request: {ex.Message}", ex);
                throw;
            }        
        }
    }
}