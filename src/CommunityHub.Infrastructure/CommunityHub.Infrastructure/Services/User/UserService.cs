using AppComponents.Repository.Abstraction;
using CommunityHub.Infrastructure.Data;
using CommunityHub.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace CommunityHub.Infrastructure.Services.User
{
    public class UserService : IUserService
    {
        private readonly ILogger<IUserService> _logger;

        private readonly IRepository<UserInfo, ApplicationDbContext> _userRepository;
        private readonly IRepository<SpouseInfo, ApplicationDbContext> _spouseRepository;
        private readonly IRepository<Children, ApplicationDbContext> _childRepository;

        public UserService(ILogger<IUserService> logger,
            IRepository<UserInfo, ApplicationDbContext> userRepository,
            IRepository<SpouseInfo, ApplicationDbContext> spouseRepository,
            IRepository<Children, ApplicationDbContext> childRepository)
        {
            _logger = logger;
            _userRepository = userRepository;
            _spouseRepository = spouseRepository;
            _childRepository = childRepository;
        }

        public async Task<UserInfo> CreateUserAsync(UserInfo userInfo, SpouseInfo? spouseInfo, List<Children>? children)
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

        public async Task<UserInfo> DeleteUsersAsync(int id)
        {
            return null;
        }


        public async Task<UserInfo> GetUserAsync(int id)
        {
            return await _userRepository.GetAsync(x => x.Id == id);
        }

        public async Task<UserInfo> GetUserAsync(Expression<Func<UserInfo, bool>> filter)
        {
            return await _userRepository.GetAsync(filter);
        }

        public async Task<List<UserInfo>> GetUsersAsync(Dictionary<string, bool>? orderBy = null)
        {
            if (orderBy == null)
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

        public Task<UserInfo> UpdateUsersAsync(UserInfo userInfo)
        {
            throw new NotImplementedException();
        }
    }
}
