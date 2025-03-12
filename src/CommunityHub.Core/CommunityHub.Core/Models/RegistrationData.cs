using CommunityHub.Core.Dtos;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommunityHub.Core.Models
{
    public class RegistrationData
    {
        [Required]
        public UserDetailsCreateDto UserDetails { get; set; }

        public SpouseDetailsCreateDto? SpouseDetails { get; set; }
        public List<string>? Children { get; set; } = new List<string>();
    }
}