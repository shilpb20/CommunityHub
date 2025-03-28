using CommunityHub.Infrastructure.Models;

namespace CommunityHub.Infrastructure.Services.User
{
    public interface IChildService
    {
        Task<Child> CreateChildAsync(Child child);
    }
}