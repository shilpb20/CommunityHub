using CommunityHub.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunityHub.Infrastructure.Services
{
    public interface ISpouseService
    {
        Task<SpouseInfo> CreateSpouseAsync(SpouseInfo spouseInfo);
        Task<SpouseInfo> GetSpouseInfoByIdAsync(int id);

        Task<SpouseInfo> GetSpouseInfoByEmail(string email);
        Task<SpouseInfo> GetSpouseInfoByPhone(string countryCode, string contactNumber);
        Task<SpouseInfo> GetSpouseInfoByUserInfoIdAsync(int userInfoId);
    }
}
