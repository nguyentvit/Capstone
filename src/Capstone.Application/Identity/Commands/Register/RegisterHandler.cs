using System.Security.Claims;
using Capstone.Application.Identity.Commands.VerifyOtpEmail;
using Capstone.Application.Identity.Extensions;
using Capstone.Application.Interface;
using Capstone.Application.Interface.Services.Identity;
using Capstone.Domain.Common.ValueObjects;
using Capstone.Domain.UserAccess.Models;
using Microsoft.AspNetCore.Identity;

namespace Capstone.Application.Identity.Commands.Register;
public class RegisterHandler(
    UserManager<ApplicationUser> userManager, 
    ISendOtpService sendOtpService,
    IApplicationDbContext dbContext) : ICommandHandler<RegisterCommand, RegisterResult>
{
    public async Task<RegisterResult> Handle(RegisterCommand command, CancellationToken cancellationToken)
    {
        var email = command.Email;
        if (await userManager.FindByEmailAsync(email) is not null)
            throw new IdentityBadRequestException("Email is already in use");
        
        var otpHasher = await sendOtpService.GetCachedReponseAsync(email);
        if (string.IsNullOrEmpty(otpHasher))
            throw new IdentityBadRequestException("Invalid otp");

        var otpVerify = System.Text.Json.JsonSerializer.Deserialize<OtpVerify>(otpHasher) ?? throw new IdentityBadRequestException("Invalid otp");

        if (!OtpExtension.VerifyOtp(command.Otp, otpVerify.otpHasher))
            throw new IdentityBadRequestException("Invalid otp");

        var userId = UserId.Of(Guid.NewGuid());

        ApplicationUser user = new()
        {
            Email = email,
            UserName = email,
            UserId = userId.Value
        };

        var result = await userManager.CreateAsync(user, command.Password);

        if (result.Succeeded)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.Role, "user")
            };

            var newUser = User.Of(userId, UserName.Of(command.Name), Email.Of(email));

            dbContext.AppUsers.Add(newUser);

            await userManager.AddToRoleAsync(user, "user");
            await userManager.AddClaimsAsync(user, claims);
            await sendOtpService.Remove(email);
            await dbContext.SaveChangesAsync(cancellationToken);


            return new RegisterResult(true);
        }

        throw new IdentityBadRequestException("Common Error");
    }
}
