namespace Capstone.Application.Identity.Commands.VerifyOtpEmail;

public class VerifyOtpEmailHandler : ICommandHandler<VerifyOtpEmailCommand, VerifyOtpEmailResult>
{
    public Task<VerifyOtpEmailResult> Handle(VerifyOtpEmailCommand command, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
