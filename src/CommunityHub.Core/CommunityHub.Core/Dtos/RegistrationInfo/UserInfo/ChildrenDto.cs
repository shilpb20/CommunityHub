using System.ComponentModel.DataAnnotations;

namespace CommunityHub.Core.Dtos
{
    public class ChildrenDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}