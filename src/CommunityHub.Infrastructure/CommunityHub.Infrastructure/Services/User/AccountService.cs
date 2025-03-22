using CommunityHub.Core.Constants;
using CommunityHub.Infrastructure.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunityHub.Infrastructure.Services.User
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public AccountService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IdentityResult> CreateUserAsync(ApplicationUser user)
        {
            var identityResult = await _userManager.CreateAsync(user);
            await _userManager.AddToRoleAsync(user, Roles.User);
            return identityResult;
        }
    }
}
