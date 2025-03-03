namespace Capstone.Application.Identity.Commands.RefreshToken;

public record RefreshTokenCommand(string RefreshToken) : ICommand<RefreshTokenResult>;
public record RefreshTokenResult(string AccessToken, string RefreshToken, int ExpiresIn, string TokenType, string IdToken);
public record TokenResult(
    [JsonProperty("access_token")]
    string AccessToken,
    [JsonProperty("refresh_token")]
    string RefreshToken,
    [JsonProperty("expires_in")]
    int ExpiresIn,
    [JsonProperty("token_type")]
    string TokenType,
    [JsonProperty("id_token")]
    string IdToken,
    [JsonProperty("sub")]
    string SubId
);
public class RefreshTokenCommandValidator : AbstractValidator<RefreshTokenCommand>
{
    public RefreshTokenCommandValidator()
    {
        RuleFor(x => x.RefreshToken)
            .NotEmpty()
            .WithMessage("Refresh Token cannot be empty");
    }
}