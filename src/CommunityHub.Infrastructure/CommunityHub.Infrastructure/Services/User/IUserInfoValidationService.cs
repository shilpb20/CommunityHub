using CommunityHub.Core.Enums;
using CommunityHub.Core.Models;

namespace CommunityHub.Infrastructure.Services.User
{
    public interface IUserInfoValidationService
    {
        Task<eDuplicateStatus> CheckDuplicateUser(UserContact userContact, UserContact? spouseContact);
    }
}