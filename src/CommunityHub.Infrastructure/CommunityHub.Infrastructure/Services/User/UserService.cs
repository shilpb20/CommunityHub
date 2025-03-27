using AppComponents.Repository.Abstraction;
using CommunityHub.Core.Dtos;
using CommunityHub.Core.Enums;
using CommunityHub.Core.Helpers;
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

        public async Task<UserInfo> GetUserAsyncById(int id)
        {
            return await _userRepository.GetAsync(x => x.Id == id);
        }

        public async Task<List<UserInfo>> GetUsersAsync(string? sortBy, bool ascending)
        {
            var orderBy = GetOrderByDictionary(sortBy, ascending);
            var users = await _userRepository.GetAll(orderByClause: orderBy);
            return await users.Include(x => x.SpouseInfo).Include(y => y.Children).ToListAsync();
        }

        private Dictionary<string, bool> GetOrderByDictionary(string? sortBy, bool ascending)
        {
            Dictionary<string, bool> orderBy = new();
            if (sortBy == null)
            {
                orderBy = new Dictionary<string, bool>
                {
                    { "Location", true },
                    { "FullName", true }
                };
            }
            else
            {
                orderBy = new Dictionary<string, bool>()
                {
                    { sortBy, ascending }
                };
            }

            return orderBy;
        }

        public async Task<eDuplicateStatus> CheckDuplicateUser(UserContactDto userContactDto, UserContactDto? spouseContact)
        {
            if (await IsDuplicateEmailAsync(userContactDto.Email))
            {
                return eDuplicateStatus.DuplicateUserEmail;
            }

            if (await IsDuplicateContactAsync(userContactDto))
            {
                return eDuplicateStatus.DuplicateUserContact;
            }

            if (spouseContact != null)
            {
                if (await IsDuplicateEmailAsync(spouseContact.Email))
                {
                    return eDuplicateStatus.DuplicateSpouseEmail;
                }

                if (await IsDuplicateContactAsync(spouseContact))
                {
                    return eDuplicateStatus.DuplicateSpouseContact;
                }
            }

            return eDuplicateStatus.NoDuplicate;
        }

        private async Task<bool> IsDuplicateEmailAsync(string email)
        {
            var user = await _userRepository.GetAsync(x => x.Email == email);
            if (user != null)
            {
                return true;
            }

            return await _spouseRepository.GetAsync(x => x.Email == email) != null;
        }

        private async Task<bool> IsDuplicateContactAsync(UserContactDto contactDto)
        {
            var user = await _userRepository.GetAsync(x => x.CountryCode == contactDto.CountryCode 
                && x.ContactNumber == contactDto.ContactNumber);
            
            if(user != null)
            {
                return true;
            }

            var spouse = await _spouseRepository.GetAsync(x => x.CountryCode == contactDto.CountryCode
                && x.ContactNumber == contactDto.ContactNumber);

            return spouse != null;
        }


        //public async Task<UserInfo> CreateUserAsync(UserInfo userInfo, SpouseInfo? spouseInfo, List<Children>? children)
        //{
        //    var newUser = await _userRepository.AddAsync(userInfo);
        //    if (newUser == null)
        //        throw new Exception("User info creation failed.");

        //    if (spouseInfo != null)
        //    {
        //        spouseInfo.UserInfoId = newUser.Id;
        //        var spouseInfoDto = await _spouseRepository.AddAsync(spouseInfo);
        //        if (spouseInfoDto == null)
        //            throw new Exception("Spouse info creation failed.");
        //    }

        //    foreach (var child in children)
        //    {
        //        child.UserInfoId = newUser.Id;
        //        await _childRepository.AddAsync(child);
        //    }

        //    return newUser;
        //}

        //public async Task<UserInfo> DeleteUsersAsync(int id)
        //{
        //    return null;
        //}

        //public async Task<UserInfo> GetUserAsync(Expression<Func<UserInfo, bool>> filter)
        //{
        //    return await _userRepository.GetAsync(filter);
        //}

        //public Task<UserInfo> UpdateUsersAsync(UserInfo userInfo)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
