namespace CommunityHub.Core.Dtos
{
    public class RegistrationInfoCreateDto
    {
        public UserInfoCreateDto UserInfo { get; set; }

        public SpouseInfoCreateDto? SpouseInfo { get; set; }
        public List<ChildrenCreateDto>? Children { get; set; } = new List<ChildrenCreateDto>();
    }
}