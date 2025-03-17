using AppComponents.Repository.Abstraction;
using CommunityHub.Core.Dtos;
using CommunityHub.Core.Enums;
using CommunityHub.Core.Models;
using CommunityHub.Infrastructure.Data;
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

        public UserService(ILogger<IUserService> logger,
            ITransactionManager transactionManager,
            IRepository<UserInfo, ApplicationDbContext> userRepository,
            IRepository<SpouseInfo, ApplicationDbContext> spouseRepository,
            IRepository<Children, ApplicationDbContext> childRepository)
        {
            _logger = logger;
            _transactionManager = transactionManager;

            _userRepository = userRepository;
            _spouseRepository = spouseRepository;
            _childRepository = childRepository;
        }

        public async Task<UserInfo> CreateUserAsync(UserInfo userInfo, SpouseInfo? spouseInfo, List<Children>? children)
        {
            using(_transactionManager.BeginTransactionAsync())
            {
                try
                {
                    if (spouseInfo != null)
                    {
                        var spouseInfoDto = await _spouseRepository.AddAsync(spouseInfo);
                        if (spouseInfoDto == null)
                            throw new Exception("Spouse info creation failed.");

                        userInfo.SpouseInfoId = spouseInfoDto.Id;
                    }

                    var newUser = await _userRepository.AddAsync(userInfo);
                    if (newUser == null)
                        throw new Exception("User info creation failed.");

                    foreach (var child in children) 
                    {
                        child.UserInfoId = newUser.Id;
                        await _childRepository.AddAsync(child);

                    }

                    await _transactionManager.CommitTransactionAsync();
                    return newUser;
                }
                catch (Exception ex)
                {
                    await _transactionManager.RollbackTransactionAsync();
                    _logger.LogError(ex, "Error creating user {userName}", userInfo.FullName);
                    throw;
                }
            }
        }

        public async Task<UserInfo> GetUserAsync(int id)
        {
            return await _userRepository.GetAsync(x => x.Id == id);
        }

        public async Task<List<UserInfo>> GetUsersAsync()
        {
            var userInfos = await _userRepository.GetAll();
            return await userInfos
                            .Include(x => x.SpouseInfo)
                            .Include(y => y.Children)
                            .ToListAsync();
        }
    }
}
