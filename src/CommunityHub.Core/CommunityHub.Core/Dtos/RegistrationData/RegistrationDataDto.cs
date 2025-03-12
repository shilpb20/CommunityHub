
using CommunityHub.Core.Dtos;

namespace CommunityHub.Core.Dtos.RegistrationData
{
    public class RegistrationDataDto
    {
        public UserDetailsDto UserDetails { get; set; }

        public SpouseDetailsDto? SpouseDetails { get; set; }
        public List<string>? Children { get; set; } = new List<string>();
    }
}