using Capstone.Application.Identity.Extensions;
using Microsoft.AspNetCore.Identity;

namespace Capstone.Application.Identity.Commands.SendOtp;
public class SenOtpHandler(
    UserManager<ApplicationUser> userManager)
 : ICommandHandler<SendOtpCommand, SendOtpResult>
{
    public async Task<SendOtpResult> Handle(SendOtpCommand command, CancellationToken cancellationToken)
    {
        var email = command.Email;
        if (await userManager.FindByEmailAsync(email) is not null)
            throw new IdentityBadRequestException("Email is already in use");

        var otp = OtpExtension.GenerateOtp();
        var otpHasher = OtpExtension.HashOtp(otp);


        throw new NotImplementedException();
    }
}
