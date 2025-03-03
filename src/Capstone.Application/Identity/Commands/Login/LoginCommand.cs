namespace Capstone.Application.Identity.Commands.Login;

public record LoginCommand(string Email, string Password, string Role) : ICommand<LoginResult>;
public record LoginResult(string AccessToken, string RefreshToken, int ExpiresIn, string TokenType, string IdToken);
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