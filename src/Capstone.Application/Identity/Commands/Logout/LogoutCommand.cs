namespace Capstone.Application.Identity.Commands.Logout;

public record LogoutCommand(string RefreshToken) : ICommand<LogoutResult>;
public record LogoutResult(bool IsSuccess);
public class LogoutCommandValidator : AbstractValidator<LogoutCommand>
{
    public LogoutCommandValidator()
    {
        RuleFor(x => x.RefreshToken)
            .NotEmpty()
            .WithMessage("Refresh Token cannot be empty");
    }
}