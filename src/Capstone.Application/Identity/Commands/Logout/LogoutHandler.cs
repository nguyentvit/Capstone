namespace Capstone.Application.Identity.Commands.Logout;
public class LogoutHandler(IConfiguration configuration) : ICommandHandler<LogoutCommand, LogoutResult>
{
    public async Task<LogoutResult> Handle(LogoutCommand command, CancellationToken cancellationToken)
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
        var client = new HttpClient();

        var token = new HttpRequestMessage(HttpMethod.Post, $"{url}connect/revocation")
        {
            Content = new FormUrlEncodedContent(
            [   new KeyValuePair<string, string>("client_id", "magic"),
                new KeyValuePair<string, string>("client_secret", "secret"),
                new KeyValuePair<string, string>("token", command.RefreshToken),
                new KeyValuePair<string, string>("token_type_hint", "refresh_token")
            ])
        };

        var tokenResponse = await client.SendAsync(token);
        if (!tokenResponse.IsSuccessStatusCode)
        {
            throw new IdentityBadRequestException("Đăng xuất không thành công");
        }

        return new LogoutResult(true);
    }
}
