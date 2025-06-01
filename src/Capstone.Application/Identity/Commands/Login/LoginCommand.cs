namespace Capstone.Application.Identity.Commands.Login;

public record LoginCommand(string UserName, string Password) : ICommand<LoginResult>;
public record LoginResult(string AccessToken, string RefreshToken, int ExpiresIn, string TokenType, string IdToken, string Role);
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
        RuleFor(x => x.UserName)
            .NotEmpty()
            .WithMessage("Tên người dùng không thể trống");

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Mật khẩu không thể trống");
    }
}