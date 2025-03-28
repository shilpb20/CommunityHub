using AppComponents.Repository.Abstraction;
using CommunityHub.Core.Constants;
using CommunityHub.Infrastructure.Models;
using CommunityHub.Infrastructure.Services.AdminService;
using CommunityHub.Infrastructure.Services.Registration;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using static CommunityHub.Core.Constants.ApiRoute;

namespace CommunityHub.Infrastructure.Services.User
{
    public class AccountService : IAccountService
    {
        private ILogger<AccountService> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
       
        public AccountService(
            ILogger<AccountService> logger,
            UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _userManager = userManager;
        }

        public async Task<ApplicationUser> CreateAccountAsync(UserInfo userInfo)
        {
            try
            {
                var applicationUser = new ApplicationUser()
                {
                    Email = userInfo.Email,
                    UserName = userInfo.Email,
                    NormalizedEmail = userInfo.Email.ToUpper(),
                    NormalizedUserName = userInfo.Email.ToUpper()
                };

                var identityResult = await _userManager.CreateAsync(applicationUser);
                if (!identityResult.Succeeded) return null;

                var roleResult = await _userManager.AddToRoleAsync(applicationUser, Roles.User);
                if (!roleResult.Succeeded) return null;

                return applicationUser;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating user {userName}", userInfo.FullName);
                throw;
            }
        }

        public async Task<string> GenerateTokenAsync(ApplicationUser applicationUser)
        {
            return await _userManager.GeneratePasswordResetTokenAsync(applicationUser);
        }


        //public async Task<IdentityResult> DeleteAccountAsync(ApplicationUser user)
        //{
        //    if (user == null)
        //    {
        //        throw new ArgumentNullException(nameof(user));
        //    }

        //    var result = await _userManager.DeleteAsync(user);
        //    if (!result.Succeeded)
        //    {
        //        return result;
        //    }

        //    return result;
        //}
    }
}
