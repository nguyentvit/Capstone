using Capstone.Application.Interface;
using Capstone.Domain.Common.ValueObjects;
using Capstone.Domain.Identity.Models;
using Capstone.Domain.UserAccess.Models;
using Capstone.Domain.UserAccess.ValueObjects;
using Microsoft.AspNetCore.Identity;

namespace Capstone.Infrastructure.Data;

public class DataSeeder
    (UserManager<ApplicationUser> userManager,
    RoleManager<IdentityRole> roleManager,
    IApplicationDbContext dbContext)
{

    public async Task SeedAsync()
    {
        await EnsureRolesAsync();
        await EnsureAdminUserAsync();
    }

    private async Task EnsureRolesAsync()
    {
        string[] roles = ["Admin", "User", "Rescue"];

        foreach (var roleName in roles)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }
    }

    private async Task EnsureAdminUserAsync()
    {
        string adminEmail = "admin@example.com";
        string adminPassword = "Admin@123";

        var adminUser = await userManager.FindByEmailAsync(adminEmail);
        if (adminUser == null)
        {
            var userId = UserId.Of(Guid.NewGuid());

            var newAdmin = new ApplicationUser
            {
                UserName = "admin",
                Email = adminEmail,
                EmailConfirmed = true,
                UserId = userId.Value
            };

            var result = await userManager.CreateAsync(newAdmin, adminPassword);
            if (result.Succeeded)
            {
                var newUser = User.Of(userId, UserName.Of("Admin"), Email.Of(adminEmail));
                dbContext.AppUsers.Add(newUser);
                
                await Task.WhenAll(
                    userManager.AddToRoleAsync(newAdmin, "Admin"),
                    dbContext.SaveChangesAsync()
                );
            }
        }
    }
}
