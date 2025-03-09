using CommunityHub.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace CommunityHub.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<RegistrationRequest> RegistrationRequests { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
    }
}