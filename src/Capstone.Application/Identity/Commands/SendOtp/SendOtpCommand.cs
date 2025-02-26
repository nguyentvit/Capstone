namespace Capstone.Application.Identity.Commands.SendOtp;

public record SendOtpCommand(string Email) : ICommand<SendOtpResult>;
public record SendOtpResult(bool IsSuccess);
public class SendOtpCommandValidator : AbstractValidator<SendOtpCommand>
{
    public SendOtpCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email cannot be empty")
            .EmailAddress()
            .WithMessage("Invalid email format");
    }
}