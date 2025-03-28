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
    public class SpouseService : ISpouseService
    {
        private readonly ILogger<ISpouseService> _logger;
        private readonly IRepository<SpouseInfo, ApplicationDbContext> _spouseRepository;

        public SpouseService(ILogger<ISpouseService> logger,
            IRepository<SpouseInfo, ApplicationDbContext> spouseRepository)
        {
            _logger = logger;
            _spouseRepository = spouseRepository;
        }

        #region get-spouse-info

        public async Task<SpouseInfo> GetSpouseInfoByIdAsync(int id)
        {
            return await _spouseRepository.GetAsync(x => x.Id == id);
        }

        public async Task<SpouseInfo> GetSpouseInfoByUserInfoIdAsync(int userInfoId)
        {
            return await _spouseRepository.GetAsync(x => x.UserInfoId == userInfoId);
        }

        public async Task<SpouseInfo> GetSpouseInfoByEmail(string email)
        {
            return await _spouseRepository.GetAsync(x => x.Email == email);
        }

        public async Task<SpouseInfo> GetSpouseInfoByPhone(string countryCode, string contactNumber)
        {
            return await _spouseRepository.GetAsync(x => x.CountryCode == countryCode && x.ContactNumber == contactNumber);
        }

        #endregion

        #region create-spouse-info
        public async Task<SpouseInfo> CreateSpouseAsync(SpouseInfo spouseInfo)
        {
            try
            {
                if (spouseInfo == null)
                {
                    return null;
                }

                if(spouseInfo.UserInfoId == 0)
                {
                    throw new Exception("User info ID is required to create a spouse.");
                }

                var newSpouseInfo =  await _spouseRepository.AddAsync(spouseInfo);
                if (newSpouseInfo == null)
                    throw new Exception("Spouse info creation failed.");

                return newSpouseInfo;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating spouse with User info id {UserId}", spouseInfo.UserInfoId);
                throw;
            }
        }

        #endregion
    }
}
