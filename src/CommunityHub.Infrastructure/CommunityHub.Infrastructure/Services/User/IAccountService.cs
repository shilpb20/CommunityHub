using CommunityHub.Infrastructure.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunityHub.Infrastructure.Services.User
{
    public interface IAccountService
    {
        public Task<IdentityResult> CreateUserAsync(ApplicationUser user);
    }
}
