using Microsoft.AspNetCore.Identity;

namespace CommunityHub.Api.Data
{
    public static class DataSeeder
    {
        public static async Task SeedRolesAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            if(!roleManager.Roles.Any())
            {
                var roles = new[] { "user", "admin", "superadmin" };
                foreach (var roleName in roles)
                {
                    var roleExists = await roleManager.RoleExistsAsync(roleName);
                    if (!roleExists)
                    {
                        var identityRole = new IdentityRole(roleName);
                        var result = await roleManager.CreateAsync(identityRole);
                        if (!result.Succeeded)
                        {
                            throw new Exception($"Failed to create role {roleName}: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                        }
                    }
                }
            }   
        }
    }
}
