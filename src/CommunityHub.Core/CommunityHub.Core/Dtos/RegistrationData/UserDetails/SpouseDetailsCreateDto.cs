using System.ComponentModel.DataAnnotations;

namespace CommunityHub.Core.Dtos
{
    public class SpouseDetailsCreateDto
    {
        [Required, MinLength(3), MaxLength(50)]
        public string FullName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string CountryCode { get; set; }

        [Required]
        public string ContactNumber { get; set; }

        [Required]
        public string HomeTown { get; set; }
        public string? HouseName { get; set; }
    }
}