namespace Capstone.Application.Identity.Commands.RefreshToken;

public record RefreshTokenCommand(string RefreshToken) : ICommand<RefreshTokenResult>;
public record RefreshTokenResult(string AccessToken, string RefreshToken, int ExpiresIn, string TokenType, string IdToken);
public class RefreshTokenCommandValidator : AbstractValidator<RefreshTokenCommand>
{
    public RefreshTokenCommandValidator()
    {
        RuleFor(x => x.RefreshToken)
            .NotEmpty()
            .WithMessage("Refresh Token cannot be empty");
    }
}