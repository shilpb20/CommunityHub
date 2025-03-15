using CommunityHub.Core.Dtos;

namespace CommunityHub.Core.Models
{
    public class RegistrationInfo
    {
        public UserInfo UserInfo { get; set; }

        public SpouseInfo? SpouseInfo { get; set; }
        public List<Children>? Children { get; set; } = new List<Children>();
    }
}