using Capstone.Application.Identity.Commands.Login;

namespace Capstone.API.Endpoints.Identity;
public record LoginRequest(string Email, string Password);
public record LoginResponse(string AccessToken, string RefreshToken, int ExpiresIn, string TokenType, string IdToken);
public class Login : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/identity/login", async (HttpContext httpContext, ISender sender, LoginRequest request) => 
        {
            var role = httpContext.Request.Headers[MiddlewareConstants.ROLE].FirstOrDefault() ?? MiddlewareConstants.DEFAULT_ROLE;

            var command = new LoginCommand(request.Email, request.Password, role);

            var result = await sender.Send(command);

            var response = result.Adapt<LoginResponse>();

            return Results.Ok(response);
        });
    }
}
