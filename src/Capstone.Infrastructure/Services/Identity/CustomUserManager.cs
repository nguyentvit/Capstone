using Capstone.Application.Interface;
using Capstone.Domain.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Capstone.Infrastructure.Services.Identity;

public class CustomUserManager(
    IUserStore<ApplicationUser> store,
    IOptions<IdentityOptions> optionsAccessor,
    IPasswordHasher<ApplicationUser> passwordHasher,
    IEnumerable<IUserValidator<ApplicationUser>> userValidators,
    IEnumerable<IPasswordValidator<ApplicationUser>> passwordValidators,
    ILookupNormalizer keyNormalizer,
    IdentityErrorDescriber errors,
    IServiceProvider services,
    ILogger<UserManager<ApplicationUser>> logger,
    IApplicationDbContext dbContext) : UserManager<ApplicationUser>(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
{
    public IApplicationDbContext DbContext { get; } = dbContext;
    public new IPasswordHasher<ApplicationUser> PasswordHasher { get; } = passwordHasher;

    public override async Task<IdentityResult> CreateAsync(ApplicationUser user, string password)
    {
        user.EmailConfirmed = true;
        var result = await base.CreateAsync(user, password);
        return result;
    }
    public async override Task<IdentityResult> ResetPasswordAsync(ApplicationUser user, string token, string newPassword)
    {
        var hashedPassword = PasswordHasher.HashPassword(user, newPassword);
        user.PasswordHash = hashedPassword;
        DbContext.ApplicationUsers.Update(user);
        await DbContext.SaveChangesAsync();
        return IdentityResult.Success;
    }
}