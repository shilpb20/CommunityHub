using CommunityHub.Core.Dtos;

namespace CommunityHub.Core
{
    public class CheckDuplicateRequestDto
    {
        public UserContactDto? Contact { get; set; }
        public UserContactDto? SpouseContact { get; set; }
    }
}