namespace Capstone.Application.Identity.Commands.Login;

public record LoginCommand(string Email, string Password) : ICommand<LoginResult>;
public record LoginResult(string AccessToken, string RefreshToken, int ExpiresIn, string TokenType, string IdToken, Guid UserId);
public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email cannot be empty")
            .EmailAddress()
            .WithMessage("Email is invalid");

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Password cannot be empty");
    }
}