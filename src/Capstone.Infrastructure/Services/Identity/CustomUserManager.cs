using Capstone.Domain.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Capstone.Infrastructure.Services.Identity;

public class CustomUserManager : UserManager<ApplicationUser>
{
    public CustomUserManager(
        IUserStore<ApplicationUser> store,
        IOptions<IdentityOptions> optionsAccessor,
        IPasswordHasher<ApplicationUser> passwordHasher,
        IEnumerable<IUserValidator<ApplicationUser>> userValidators,
        IEnumerable<IPasswordValidator<ApplicationUser>> passwordValidators,
        ILookupNormalizer keyNormalizer,
        IdentityErrorDescriber errors,
        IServiceProvider services,
        ILogger<UserManager<ApplicationUser>> logger)
        : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
    {
    }

    public override async Task<IdentityResult> CreateAsync(ApplicationUser user, string password)
    {
        user.UserName = user.Email;

        user.EmailConfirmed = true;
        var result = await base.CreateAsync(user, password);
        return result;
    }
}