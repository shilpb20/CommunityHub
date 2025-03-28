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
using Newtonsoft.Json.Linq;

namespace CommunityHub.Infrastructure.Services.AdminService
{
    public class AdminService : IAdminService
    {
        private readonly ILogger<IAdminService> _logger;
        private readonly ITransactionManager _transactionManager;
        private readonly IRegistrationService _registrationService;
        private readonly IAccountService _accountService;
        private readonly IUserService _userService;
        private readonly ISpouseService _spouseService;
        private readonly IChildService _childService;
        private readonly IAppMailService _mailService;

        public AdminService(
            ILogger<IAdminService> logger,
            ITransactionManager transactionManager,
            IRegistrationService registrationService,
            IAccountService accountService,
            IUserService userService,
            ISpouseService spouseService,
            IChildService childService,
            IAppMailService mailService)
        {
            _logger = logger;
            _transactionManager = transactionManager;
            _registrationService = registrationService;
            _accountService = accountService;
            _userService = userService;
            _spouseService = spouseService;
            _childService = childService;
            _mailService = mailService;
        }

        public async Task<UserInfo> ApproveRequestAsync(RegistrationRequest registrationRequest)
        {
            await _transactionManager.BeginTransactionAsync();
            try
            {
                var matchingRequest = await _registrationService.GetRequestByIdAsync(registrationRequest.Id);
                if (matchingRequest == null) return null;

                var registrationInfo = JsonConvert.DeserializeObject<RegistrationInfo>(matchingRequest.RegistrationInfo);

                var applicationUser = await _accountService.CreateAccountAsync(registrationInfo.UserInfo);
                if (applicationUser == null)
                {
                    await _transactionManager.RollbackTransactionAsync();
                    return null;
                };

                var userInfo = registrationInfo.UserInfo;
                userInfo.ApplicationUserId = applicationUser.Id;
                var newUser = await _userService.CreateUserAsync(userInfo);

                var spouseInfo = registrationInfo.SpouseInfo;
                if (spouseInfo != null)
                {
                    spouseInfo.UserInfoId = newUser.Id;
                    await _spouseService.CreateSpouseAsync(spouseInfo);
                }

                var children = registrationInfo.Children;
                if (children.Any())
                {
                    foreach (var child in children)
                    {
                        child.UserInfoId = newUser.Id;
                        await _childService.CreateChildAsync(child);
                    }
                }

                matchingRequest.Approve();
                await _registrationService.UpdateRequestAsync(matchingRequest);

                await _transactionManager.CommitTransactionAsync();

                string token = await _accountService.GenerateTokenAsync(applicationUser);
                var model = new RegistrationApprovalModel()
                { 
                    UserName = userInfo.FullName,
                    Email = userInfo.Email,
                    PasswordSetLink = token,
                };

                var emailStatus = await _mailService.SendRegistrationRequestApprovalNotificationAsync(model);
                return newUser;
            }
            catch (Exception ex)
            {
                await _transactionManager.RollbackTransactionAsync();
                _logger.LogError(ex, "Error approving request {Id}", registrationRequest.Id);
                throw;
            }
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