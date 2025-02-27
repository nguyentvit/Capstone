using System.Security.Claims;
using System.Text.Json;
using Capstone.Application.Identity.Commands.VerifyOtpEmail;
using Capstone.Application.Identity.Extensions;
using Capstone.Application.Interface.Services.Identity;
using Microsoft.AspNetCore.Identity;

namespace Capstone.Application.Identity.Commands.Register;
public class RegisterHandler(
    UserManager<ApplicationUser> userManager, 
    ISendOtpService sendOtpService) : ICommandHandler<RegisterCommand, RegisterResult>
{
    public async Task<RegisterResult> Handle(RegisterCommand command, CancellationToken cancellationToken)
    {
        var email = command.Email;
        if (await userManager.FindByEmailAsync(email) is not null)
            throw new IdentityBadRequestException("Email is already in use");
        
        var otpHasher = await sendOtpService.GetCachedReponseAsync(email);
        if (string.IsNullOrEmpty(otpHasher))
            throw new IdentityBadRequestException("Invalid otp");

        var otpVerify = JsonSerializer.Deserialize<OtpVerify>(otpHasher) ?? throw new IdentityBadRequestException("Invalid otp");

        if (!OtpExtension.VerifyOtp(command.Otp, otpVerify.otpHasher))
            throw new IdentityBadRequestException("Invalid otp");

        ApplicationUser user = new()
        {
            Email = email,
            UserName = email
        };

        var result = await userManager.CreateAsync(user, command.Password);

        if (result.Succeeded)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.Role, IdentityConfig.User)
            };

            await userManager.AddToRoleAsync(user, IdentityConfig.User);
            await userManager.AddClaimsAsync(user, claims);
            await sendOtpService.Remove(email);

            return new RegisterResult(true);
        }

        throw new IdentityBadRequestException("Common Error");
    }
}
