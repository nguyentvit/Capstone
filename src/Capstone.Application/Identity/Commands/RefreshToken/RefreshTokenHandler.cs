namespace Capstone.Application.Identity.Commands.RefreshToken;
public class RefreshTokenHandler(IConfiguration configuration) : ICommandHandler<RefreshTokenCommand, RefreshTokenResult>
{
    public async Task<RefreshTokenResult> Handle(RefreshTokenCommand command, CancellationToken cancellationToken)
    {
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

        var refreshToken = command.RefreshToken;

        var client = new HttpClient();
        var token = new HttpRequestMessage(HttpMethod.Post, $"{url}connect/token")
        {
            Content = new FormUrlEncodedContent(
            [
                new KeyValuePair<string, string>("client_id", "magic"),
                new KeyValuePair<string, string>("client_secret", "secret"),
                new KeyValuePair<string, string>("grant_type", "refresh_token"),
                new KeyValuePair<string, string>("refresh_token", refreshToken)
            ])
        };
        var tokenResponse = await client.SendAsync(token);
        if (!tokenResponse.IsSuccessStatusCode)
        {
            throw new IdentityBadRequestException("Invalid grand"); ;
        }

        var tokenContent = await tokenResponse.Content.ReadAsStringAsync();
        var tokenResult = JsonConvert.DeserializeObject<TokenResult>(tokenContent);

        RefreshTokenResult result = new(tokenResult!.AccessToken, tokenResult.RefreshToken, tokenResult.ExpiresIn, tokenResult.TokenType, tokenResult.IdToken);

        return result;
    }
}
