using AppComponents.Repository.Abstraction;
using CommunityHub.Core.Dtos;
using CommunityHub.Core.Enums;
using CommunityHub.Infrastructure.Data;
using CommunityHub.Infrastructure.Models;
using Microsoft.AspNetCore.Mvc.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace CommunityHub.Infrastructure.Services.User
{
    public class UserService : IUserService
    {
        private readonly ILogger<IUserService> _logger;
        private readonly ITransactionManager _transactionManager;

        private readonly IRepository<UserInfo, ApplicationDbContext> _userRepository;
        private readonly IRepository<SpouseInfo, ApplicationDbContext> _spouseRepository;
        private readonly IRepository<Children, ApplicationDbContext> _childRepository;
        private readonly IAccountService _accountService;

        public UserService(ILogger<IUserService> logger,
            ITransactionManager transactionManager,
            IRepository<UserInfo, ApplicationDbContext> userRepository,
            IRepository<SpouseInfo, ApplicationDbContext> spouseRepository,
            IRepository<Children, ApplicationDbContext> childRepository,
            IAccountService accountService)
        {
            _logger = logger;
            _transactionManager = transactionManager;

            _userRepository = userRepository;
            _spouseRepository = spouseRepository;
            _childRepository = childRepository;
            _accountService = accountService;
        }

        public async Task<UserInfo> CreateUserAsync(UserInfo userInfo, SpouseInfo? spouseInfo, List<Children>? children)
        {
            using (_transactionManager.BeginTransactionAsync())
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

                    var identityResult = await _accountService.CreateUserAsync(applicationUser);
                    if (identityResult.Succeeded)
                    {
                        userInfo.ApplicationUserId = applicationUser.Id;
                        var newUser = await CreateUser(userInfo, spouseInfo, children);
                        await _transactionManager.CommitTransactionAsync();
                        return newUser;
                    }
                    else
                    {
                        throw new Exception("Failed to create application user");
                    } 
                }
                catch (Exception ex)
                {
                    await _transactionManager.RollbackTransactionAsync();
                    _logger.LogError(ex, "Error creating user {userName}", userInfo.FullName);
                    throw;
                }
            }
        }

        private async Task<UserInfo?> CreateUser(UserInfo userInfo, SpouseInfo? spouseInfo, List<Children>? children)
        {
            var newUser = await _userRepository.AddAsync(userInfo);
            if (newUser == null)
                throw new Exception("User info creation failed.");

            if (spouseInfo != null)
            {
                spouseInfo.UserInfoId = newUser.Id;
                var spouseInfoDto = await _spouseRepository.AddAsync(spouseInfo);
                if (spouseInfoDto == null)
                    throw new Exception("Spouse info creation failed.");


            }

            foreach (var child in children)
            {
                child.UserInfoId = newUser.Id;
                await _childRepository.AddAsync(child);
            }

            return newUser;
        }

        public async Task<UserInfo> GetUserAsync(int id)
        {
            return await _userRepository.GetAsync(x => x.Id == id);
        }

        public async Task<List<UserInfo>> GetUsersAsync(Dictionary<string, bool>? orderBy = null)
        {
            if(orderBy == null)
            {
                orderBy = new Dictionary<string, bool>
                {
                    { "Location", true },
                    { "FullName", true }
                };
            }

            var users = await _userRepository.GetAll(orderByClause: orderBy);
            return await users.Include(x => x.SpouseInfo).Include(y => y.Children).ToListAsync();
        }
    }
}
