using Capstone.Application.Identity.Commands.RefreshToken;

namespace Capstone.API.Endpoints.Identity;
public record RefreshTokenRequest(string RefreshToken);
public record RefreshTokenResponse(string AccessToken, string RefreshToken, int ExpiresIn, string TokenType, string IdToken);
public class RefreshToken : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/identity/refresh", async (ISender sender, RefreshTokenRequest request) => {
            
            var command = new RefreshTokenCommand(request.RefreshToken);

            var result = await sender.Send(command);

            var response = result.Adapt<RefreshTokenResponse>();

            return Results.Ok(response);
        });
    }
}
