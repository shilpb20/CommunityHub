using AppComponents.Repository.Abstraction;
using CommunityHub.Core.Enums;
using CommunityHub.Core.Extensions;
using CommunityHub.Infrastructure.Models;
using CommunityHub.Infrastructure.Data;
using CommunityHub.Infrastructure.Services.User;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace CommunityHub.Infrastructure.Services.Registration
{
    public class AdminService : IAdminService
    {
        private readonly ILogger<IAdminService> _logger;
        private readonly ITransactionManager _transactionManager;
        private readonly IAccountService _accountService;

        private readonly IRegistrationService _registrationService;

        public AdminService(
            ILogger<IAdminService> logger,
            ITransactionManager transactionManager,
            IAccountService accountService,
            IRegistrationService registrationService)
        {
            _logger = logger;
            _transactionManager = transactionManager;

            _accountService = accountService;
            _registrationService = registrationService;
        }

        public async Task<UserInfo> ApproveRequestAsync(int id)
        {
            using (_transactionManager.BeginTransactionAsync())
            {
                try
                {
                    var matchingRequest = await _registrationService.GetRequestByIdAsync(id);
                    if (matchingRequest == null) return null;

                    return null;
                    //var registrationInfo = JsonConvert.DeserializeObject<RegistrationInfo>(matchingRequest.RegistrationInfo);
                    //var newUser = await _userService.CreateUserAsync(registrationInfo.UserInfo, registrationInfo.SpouseInfo, registrationInfo.Children);
                    //if (newUser == null) return null;

                    //matchingRequest.ReviewedAt = DateTime.UtcNow;
                    //matchingRequest.RegistrationStatus = RegistrationStatus.Approved.GetEnumMemberValue();
                    //await _requestRepository.UpdateAsync(matchingRequest);

                    //await _transactionManager.CommitTransactionAsync();
                    //return newUser;
                }
                catch (Exception ex)
                {
                    await _transactionManager.RollbackTransactionAsync();
                    _logger.LogError(ex, "Error approving request {Id}", id);
                    throw;
                }
            }
        }

        public async Task<RegistrationRequest> RejectRequestAsync(int id, string reviewComment)
        {
            var registrationRequest = await _registrationService.GetRequestByIdAsync(id);
            if (registrationRequest == null) return null;

            registrationRequest.Review = reviewComment;
            registrationRequest.SetStatusToRejected();
            registrationRequest.SetReviewedTime();

            return await _registrationService.UpdateRequestAsync(registrationRequest);
        }
    }
}