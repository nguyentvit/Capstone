using System.Text.Json;
using Capstone.Application.Identity.Extensions;
using Capstone.Application.Interface.Services.Identity;

namespace Capstone.Application.Identity.Commands.VerifyOtpEmail;

public record OtpVerify(string otpHasher, string status);
public class VerifyOtpEmailHandler(ISendOtpService sendOtpService) : ICommandHandler<VerifyOtpEmailCommand, VerifyOtpEmailResult>
{
    public async Task<VerifyOtpEmailResult> Handle(VerifyOtpEmailCommand command, CancellationToken cancellationToken)
    {
        var email = command.Email;
        var otp = command.Otp;

        var otpHasher = await sendOtpService.GetCachedReponseAsync(email);
        if (string.IsNullOrEmpty(otpHasher))
            throw new IdentityBadRequestException("Invalid Otp");

        var otpVerify = System.Text.Json.JsonSerializer.Deserialize<OtpVerify>(otpHasher) ?? throw new IdentityBadRequestException("Invalid Otp");

        if (!OtpExtension.VerifyOtp(otp, otpVerify.otpHasher))
            throw new IdentityBadRequestException("Invalid Otp");

        var obj = new Dictionary<string, string>
        {
            { "OtpHasher", otpVerify.otpHasher },
            { "Status", "true" }
        };

        await sendOtpService.SetCacheReponseAsync(command.Email, obj, TimeSpan.FromSeconds(300));

        return new VerifyOtpEmailResult(true);
        
    }
}
