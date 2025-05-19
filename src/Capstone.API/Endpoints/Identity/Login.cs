using Capstone.Application.Identity.Commands.Login;

namespace Capstone.API.Endpoints.Identity;
public record LoginRequest(string UserName, string Password);
public record LoginResponse(string AccessToken, string RefreshToken, int ExpiresIn, string TokenType, string IdToken, string Role);
public class Login : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/identity/login", async (ISender sender, LoginRequest request) => 
        {
            var command = new LoginCommand(request.UserName, request.Password);

            var result = await sender.Send(command);

            var response = result.Adapt<LoginResponse>();

            return Results.Ok(response);
        });
    }
}
