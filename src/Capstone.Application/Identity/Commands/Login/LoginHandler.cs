using Microsoft.AspNetCore.Identity;

namespace Capstone.Application.Identity.Commands.Login;
public class LoginHandler(
    IConfiguration configuration,
    UserManager<ApplicationUser> userManager
) : ICommandHandler<LoginCommand, LoginResult>
{
    public async Task<LoginResult> Handle(LoginCommand command, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByNameAsync(command.UserName);
        if (user == null)
            throw new IdentityBadRequestException("Tài khoản hoặc mật khẩu không chính xác");

        var url = "";
        var environment = configuration["ASPNETCORE_ENVIRONMENT"];
        if (environment == "Development")
        {
            url = "http://localhost:5036/";
        }
        else if (environment == "Production")
        {
            var port = Environment.GetEnvironmentVariable("PORT") ?? "80";
            url = $"http://identity.api:{port}/";
        }

        var client = new HttpClient();
        var token = new HttpRequestMessage(HttpMethod.Post, $"{url}connect/token")
        {
            Content = new FormUrlEncodedContent(
            [
                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("username", command.UserName),
                new KeyValuePair<string, string>("password", command.Password),
                new KeyValuePair<string, string>("client_id", "magic"),
                new KeyValuePair<string, string>("client_secret", "secret"),
                new KeyValuePair<string, string>("scope", "email openid profile offline_access")
            ])
        };
        var tokenResponse = await client.SendAsync(token);
        if (!tokenResponse.IsSuccessStatusCode)
        {
            throw new IdentityBadRequestException("Tài khoản hoặc mật khẩu không chính xác");
        }

        var tokenContent = await tokenResponse.Content.ReadAsStringAsync();
        var tokenResult = JsonConvert.DeserializeObject<TokenResult>(tokenContent) ?? throw new IdentityBadRequestException("Bad request");
        var roles = await userManager.GetRolesAsync(user);

        return new LoginResult(
            AccessToken: tokenResult.AccessToken,
            RefreshToken: tokenResult.RefreshToken,
            ExpiresIn: tokenResult.ExpiresIn,
            TokenType: tokenResult.TokenType,
            IdToken: tokenResult.IdToken,
            Role: roles.First()
        );
    }
}
