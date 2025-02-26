namespace Capstone.Application.Identity.Commands.VerifyOtpEmail;
public record VerifyOtpEmailCommand(string Email, string Otp) : ICommand<VerifyOtpEmailResult>;
public record VerifyOtpEmailResult(bool IsSuccess);
public class VerifyOtpEmailCommandValidator : AbstractValidator<VerifyOtpEmailCommand>
{
    public VerifyOtpEmailCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email cannot be empty")
            .EmailAddress()
            .WithMessage("Invalid email format");

        RuleFor(x => x.Otp)
            .NotEmpty()
            .Length(6)
            .WithMessage("Length otp is 6");
    }
}