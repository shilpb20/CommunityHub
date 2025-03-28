using AppComponents.Email.Services;
using AppComponents.Repository.Abstraction;
using CommunityHub.Core.Constants;
using CommunityHub.Core.Enums;
using CommunityHub.Core.Extensions;
using CommunityHub.Infrastructure.Data;
using CommunityHub.Infrastructure.EmailSenderService;
using CommunityHub.Infrastructure.EmailService;
using CommunityHub.Infrastructure.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace CommunityHub.Infrastructure.Services.Registration
{
    public class RegistrationService : IRegistrationService
    {
        private readonly ILogger<IRegistrationService> _logger;
        private readonly IAppMailService _appMailService;
        private readonly IRepository<RegistrationRequest, ApplicationDbContext> _repository;

        public RegistrationService(
            ILogger<IRegistrationService> logger,
            IAppMailService appMailService,
            IRepository<RegistrationRequest, ApplicationDbContext> repository)
        {
            _logger = logger;
            _appMailService = appMailService;
            _repository = repository;
        }

        public async Task<RegistrationRequest> CreateRequestAsync(RegistrationInfo registrationData)
        {
            try
            {
                if (registrationData == null)
                {
                    _logger.LogDebug("Registration data null. Skipping request creation");
                    return null;
                }

                _logger.LogDebug($"Creating registration request for user - {registrationData?.UserInfo.FullName}.");

                var registrationInfo = JsonConvert.SerializeObject(registrationData);
                var registrationRequest = new RegistrationRequest(registrationInfo);

                var newRequest = await _repository.AddAsync(registrationRequest);


                var model = new RegistrationModel()
                {
                    Id = newRequest.Id,
                    Location = registrationData.UserInfo.Location,
                    UserName = registrationData.UserInfo.FullName,
                    RegistrationDate = newRequest.CreatedAt
                };  

                await _appMailService.SendRegistrationNotificationAsync(model);

                return newRequest;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while creating registration request: {ex.Message}", ex);
                throw;
            }
        }

        public async Task<RegistrationRequest> GetRequestByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning($"Invalid ID: {id}. Cannot retrieve registration request.");
                return null;
            }

            return await _repository.GetAsync(x => x.Id == id);
        }

        public async Task<List<RegistrationRequest>> GetRequestsAsync(eRegistrationStatus status = eRegistrationStatus.Pending)
        {
            if (status == eRegistrationStatus.All)
            {
                return await _repository.GetAllAsync();
            }

            return await _repository.GetAllAsync(x => x.RegistrationStatus == status.GetEnumMemberValue());
        }

        public async Task<RegistrationRequest> UpdateRequestAsync(RegistrationRequest registrationRequest)
        {
            return await _repository.UpdateAsync(registrationRequest);
        }
    }
}
