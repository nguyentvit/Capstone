using Microsoft.AspNetCore.Identity;

namespace Capstone.Application.Identity.Commands.Login;
public class LoginHandler(
    IConfiguration configuration,
    UserManager<ApplicationUser> userManager
) : ICommandHandler<LoginCommand, LoginResult>
{
    public async Task<LoginResult> Handle(LoginCommand command, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(command.Email);
        if (user == null)
            throw new IdentityBadRequestException("Invalid username or password");

        var roles = await userManager.GetRolesAsync(user);

        if (!roles.Contains(command.Role))
        {
            throw new UnauthorizedAccessException("Invalid username or password");
        }

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
                new KeyValuePair<string, string>("username", command.Email),
                new KeyValuePair<string, string>("password", command.Password),
                new KeyValuePair<string, string>("client_id", "magic"),
                new KeyValuePair<string, string>("client_secret", "secret"),
                new KeyValuePair<string, string>("scope", "email openid profile offline_access")
            ])
        };
        var tokenResponse = await client.SendAsync(token);
        if (!tokenResponse.IsSuccessStatusCode)
        {
            throw new IdentityBadRequestException("Invalid username or password");
        }

        var tokenContent = await tokenResponse.Content.ReadAsStringAsync();
        var tokenResult = JsonConvert.DeserializeObject<TokenResult>(tokenContent) ?? throw new IdentityBadRequestException("Bad request");

        return new LoginResult(
            AccessToken: tokenResult.AccessToken,
            RefreshToken: tokenResult.RefreshToken,
            ExpiresIn: tokenResult.ExpiresIn,
            TokenType: tokenResult.TokenType,
            IdToken: tokenResult.IdToken
        );
    }
}
