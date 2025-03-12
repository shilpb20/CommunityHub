using System.ComponentModel.DataAnnotations;

namespace CommunityHub.Core.Dtos
{
    public class SpouseDetailsDto
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string CountryCode { get; set; }
        public string ContactNumber { get; set; }
        public string HomeTown { get; set; }
        public string? HouseName { get; set; }
    }
}