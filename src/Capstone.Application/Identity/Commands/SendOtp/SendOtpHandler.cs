using Capstone.Application.Common.DomainEvents;
using Capstone.Application.Identity.Extensions;
using Capstone.Application.Interface.Services.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Capstone.Application.Identity.Commands.SendOtp;
public class SenOtpHandler(
    UserManager<ApplicationUser> userManager,
    ISendOtpService sendOtpService,
    IPublisher publisher)
 : ICommandHandler<SendOtpCommand, SendOtpResult>
{
    public async Task<SendOtpResult> Handle(SendOtpCommand command, CancellationToken cancellationToken)
    {
        var email = command.Email;
        if (await userManager.FindByEmailAsync(email) is not null)
            throw new IdentityBadRequestException("Email is already in use");

        var otp = OtpExtension.GenerateOtp();
        var otpHasher = OtpExtension.HashOtp(otp);

        var obj = new Dictionary<string, string>
        {
            { "otpHasher", otpHasher },
            { "status", "false" }
        };

        await Task.WhenAll(
            sendOtpService.SetCacheReponseAsync(command.Email, obj, TimeSpan.FromSeconds(300)),
            publisher.Publish(new SendEmailDomainEvent(command.Email, otp), cancellationToken: default)
        );

        return new SendOtpResult(true);
    }
}
