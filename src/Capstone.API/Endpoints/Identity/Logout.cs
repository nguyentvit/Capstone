using Capstone.Application.Identity.Commands.Logout;

namespace Capstone.API.Endpoints.Identity;
public record LogoutRequest(string RefreshToken);
public record LogoutResponse(bool IsSuccess);
public class Logout : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/identity/logout", async (ISender sender, LogoutRequest request) => {

            var command = new LogoutCommand(request.RefreshToken);

            var result = await sender.Send(command);

            var response = result.Adapt<LogoutResponse>();

            return Results.Ok(response);
        });
    }
}
