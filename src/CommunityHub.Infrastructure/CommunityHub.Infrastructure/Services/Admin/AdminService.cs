using AppComponents.Repository.Abstraction;
using CommunityHub.Core.Enums;
using CommunityHub.Core.Extensions;
using CommunityHub.Infrastructure.Models;
using CommunityHub.Infrastructure.Data;
using CommunityHub.Infrastructure.Services.User;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using CommunityHub.Infrastructure.Services.Registration;
using CommunityHub.Infrastructure.EmailService;
using CommunityHub.Core.Models;
using CommunityHub.Infrastructure.AppMailService;

namespace CommunityHub.Infrastructure.Services.AdminService
{
    public class AdminService : IAdminService
    {
        private readonly ILogger<IAdminService> _logger;
        private readonly ITransactionManager _transactionManager;
        private readonly IRegistrationService _registrationService;
        private readonly IUserService _userService;
        private readonly IAppMailService _mailService;

        public AdminService(
            ILogger<IAdminService> logger,
            ITransactionManager transactionManager,
            IRegistrationService registrationService,
            IUserService userService,
            IAppMailService mailService)
        {
            _logger = logger;
            _transactionManager = transactionManager;
            _registrationService = registrationService;
            _userService = userService;
            _mailService = mailService;
        }

        public async Task<UserInfo> ApproveRequestAsync(int id)
        {
            return null;
            //using (_transactionManager.BeginTransactionAsync())
            //{
            //    try
            //    {
            //        var matchingRequest = await _registrationService.GetRequestByIdAsync(id);
            //        if (matchingRequest == null) return null;

            //        return null;
            //        //var registrationInfo = JsonConvert.DeserializeObject<RegistrationInfo>(matchingRequest.RegistrationInfo);
            //        //var newUser = await _userService.CreateUserAsync(registrationInfo.UserInfo, registrationInfo.SpouseInfo, registrationInfo.Children);
            //        //if (newUser == null) return null;

            //        //matchingRequest.ReviewedAt = DateTime.UtcNow;
            //        //matchingRequest.RegistrationStatus = RegistrationStatus.Approved.GetEnumMemberValue();
            //        //await _requestRepository.UpdateAsync(matchingRequest);

            //        //await _transactionManager.CommitTransactionAsync();
            //        //return newUser;
            //    }
            //    catch (Exception ex)
            //    {
            //        await _transactionManager.RollbackTransactionAsync();
            //        _logger.LogError(ex, "Error approving request {Id}", id);
            //        throw;
            //    }
            //}
        }

        public async Task<RegistrationRequest> RejectRequestAsync(int id, string reviewComment)
        {
            var registrationRequest = await _registrationService.GetRequestByIdAsync(id);
            if (registrationRequest == null) return null;

            registrationRequest.Reject(reviewComment);
            var result = await _registrationService.UpdateRequestAsync(registrationRequest);

            var registrationInfo = JsonConvert.DeserializeObject<RegistrationInfo>(registrationRequest.RegistrationInfo);
            var registrationRejectModel = new RegistrationRejectModel()
            { 
                  Email = registrationInfo.UserInfo.Email,
                  UserName = registrationInfo.UserInfo.FullName, 
                  RejectionReason = reviewComment,
                  Location = registrationInfo.UserInfo.Location,
                  RegistrationDate = registrationRequest.CreatedAt.ToString("dd MMM yyyy")
            };

            var emailStatus = await _mailService.SendRegistrationRequestRejectionNotificationAsync(registrationRejectModel);
            return result;
        }
    }
}