using CommunityHub.Core.Dtos.RegistrationData;
using System.ComponentModel.DataAnnotations;

namespace CommunityHub.Core.Dtos
{
    public class RegistrationRequestDto
    {
        public int Id { get; set; }

        public RegistrationDataDto RegistrationData { get; set; }
        public DateTime CreatedAt { get; set; }
        public string RegistrationStatus { get; set; }
        public DateTime? ReviewedAt { get; set; }
        public string? Review { get; set; }
    }
}