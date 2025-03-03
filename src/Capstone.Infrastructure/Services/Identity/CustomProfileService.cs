using System.Security.Claims;
using Capstone.Domain.Identity.Models;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using IdentityModel;
using Microsoft.AspNetCore.Identity;

namespace Capstone.Infrastructure.Services.Identity;

public class CustomProfileService(UserManager<ApplicationUser> userManager) : IProfileService
{
    public async Task GetProfileDataAsync(ProfileDataRequestContext context)
    {
        var email = context.Subject.Claims.FirstOrDefault(c => c.Type == JwtClaimTypes.Email)?.Value;
        var sub = context.Subject.Claims.FirstOrDefault(c => c.Type == JwtClaimTypes.Subject)?.Value;


        var user = string.IsNullOrEmpty(email) ? await userManager.FindByIdAsync(sub) : await userManager.FindByEmailAsync(email);
        var userId = user!.UserId;

        var claims = await userManager.GetClaimsAsync(user);

        var roles = await userManager.GetRolesAsync(user);
        claims.Add(new Claim(JwtClaimTypes.Id, userId.ToString()));
        
        foreach (var role in roles)
        {
            claims.Add(new Claim(JwtClaimTypes.Role, role));
        }

        context.IssuedClaims = (List<Claim>)claims;
    }

    public Task IsActiveAsync(IsActiveContext context)
    {
        context.IsActive = true;
        return Task.CompletedTask;
    }
}
