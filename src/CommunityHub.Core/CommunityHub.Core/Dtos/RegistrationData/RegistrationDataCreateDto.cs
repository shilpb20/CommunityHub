
using System.ComponentModel.DataAnnotations;

namespace CommunityHub.Core.Dtos
{
    public class RegistrationDataCreateDto
    {
        [Required]
        public UserDetailsCreateDto UserDetails { get; set; }
        public SpouseDetailsCreateDto? SpouseDetails { get; set; }
        public List<string>? Children { get; set; }
    }
}