using AppComponents.Repository.Abstraction;
using CommunityHub.Core.Constants;
using CommunityHub.Infrastructure.Models;
using CommunityHub.Infrastructure.Services.Registration;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace CommunityHub.Infrastructure.Services.User
{
    public class AccountService : IAccountService
    {
        private ILogger<AccountService> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAdminService _registrationService;
        private readonly IUserService _userService;
        private readonly ITransactionManager _transactionManager;
       
        public AccountService(
            ILogger<AccountService> logger,
            ITransactionManager transactionManager,
            IAdminService registrationService,
            IUserService userService,
            UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _registrationService = registrationService;
            _userService = userService;
            _userManager = userManager;
        }

        public async Task<UserInfo> CreateAccountAsync()
        {
            using (_transactionManager.BeginTransactionAsync())
            {
                try
                {

                    var applicationUser = new ApplicationUser()
                    {
                        //Email = userInfo.Email,
                        //UserName = userInfo.Email,
                        //NormalizedEmail = userInfo.Email.ToUpper(),
                        //NormalizedUserName = userInfo.Email.ToUpper()
                    };

                    var result = await CreateUserAsync(applicationUser);
                    if (result.Succeeded) 
                    {
                       await _transactionManager.CommitTransactionAsync();
                    }
                }
                catch (Exception ex)
                {
                    await _transactionManager.RollbackTransactionAsync();
                    //_logger.LogError(ex, "Error creating user {userName}", userInfo.FullName);
                    throw;
                }

                return null;
            }
        }

        private async Task<IdentityResult> CreateUserAsync(ApplicationUser user)
        {
            var identityResult = await _userManager.CreateAsync(user);
            if (!identityResult.Succeeded) return identityResult;

            var roleResult = await _userManager.AddToRoleAsync(user, Roles.User);
            if (!roleResult.Succeeded) return roleResult;

            return identityResult;
        }

        public async Task<IdentityResult> DeleteAccountAsync(ApplicationUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                return result;
            }

            return result;
        }

        Task<IdentityResult> IAccountService.CreateUserAsync(ApplicationUser user)
        {
            throw new NotImplementedException();
        }
    }
}
