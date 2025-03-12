namespace CommunityHub.Core.Dtos
{
    public class UserDetailsDto
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string CountryCode { get; set; }
        public string ContactNumber { get; set; }
        public string Gender { get; set; }
        public string Location { get; set; }
        public string MaritalStatus { get; set; }
        public string HomeTown { get; set; }
        public string? HouseName { get; set; }
    }
}