using Microsoft.AspNetCore.Identity;

namespace Capstone.Domain.Identity.Models;

public class ApplicationUser : IdentityUser
{
    public Guid UserId { get; set; } = default!;
}