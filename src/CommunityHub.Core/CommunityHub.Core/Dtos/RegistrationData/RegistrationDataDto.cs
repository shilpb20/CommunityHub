
namespace CommunityHub.Core.Dtos.RegistrationData
{
    public class RegistrationDataDto
    {
        public UserDetailsCreateDto UserDetails { get; set; }

        public SpouseDetailsCreateDto? SpouseDetails { get; set; }
        public List<string>? Children { get; set; }
    }
}