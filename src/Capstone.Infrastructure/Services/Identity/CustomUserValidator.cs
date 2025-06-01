using Capstone.Domain.Identity.Models;
using Microsoft.AspNetCore.Identity;

namespace Capstone.Infrastructure.Services.Identity
{
    public class CustomUserValidator : IUserValidator<ApplicationUser>
    {
        public async Task<IdentityResult> ValidateAsync(UserManager<ApplicationUser> manager, ApplicationUser user)
        {
            if (user == null)
                return IdentityResult.Failed(new IdentityError { Description = "Người dùng không hợp lệ." });

            if (!user.IsActive)
                return IdentityResult.Failed(new IdentityError { Description = "Tài khoản này đã bị vô hiệu hóa." });

            await Task.CompletedTask;

            return IdentityResult.Success;
        }
    }
}
