using CommunityHub.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Newtonsoft.Json;

namespace CommunityHub.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<RegistrationRequest> RegistrationRequests { get; set; }
        public DbSet<UserInfo> UserInfo { get; set; }
        public DbSet<SpouseInfo> SpouseInfo { get; set; }
        public DbSet<Children> Children { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserInfo>()
           .HasIndex(u => u.Email)
           .IsUnique();

            modelBuilder.Entity<UserInfo>()
                .HasIndex(u => u.ContactNumber)
                .IsUnique();

            modelBuilder.Entity<SpouseInfo>()
            .HasIndex(s => s.Email)
            .IsUnique();

            modelBuilder.Entity<SpouseInfo>()
                .HasIndex(s => s.ContactNumber)
                .IsUnique();

            // Relationships
            modelBuilder.Entity<UserInfo>()
                .HasOne(r => r.SpouseInfo)
                .WithMany()
                .HasForeignKey(r => r.SpouseInfoId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Children>()
                .HasOne(c => c.UserInfo)
                .WithMany(r => r.Children)
                .HasForeignKey(c => c.UserInfoId)
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);
        }
    }
}