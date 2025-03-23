using System.ComponentModel.DataAnnotations;

namespace CommunityHub.Core.Dtos.RegistrationInfo.UserInfo
{
    public class FamilyPictureCreateDto
    {
        [Required]
        public string ImageUrl { get; set; }
    }
}
