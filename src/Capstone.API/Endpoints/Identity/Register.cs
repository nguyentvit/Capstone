using Capstone.Application.Identity.Commands.Register;

namespace Capstone.API.Endpoints.Identity;
public record RegisterRequest(string Email, string Name, string Password, string ConfirmPassword, string Otp);
public record RegisterResponse(bool IsSuccess);
public class Register : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/identity/register", async (ISender sender, RegisterRequest request) => {

            var command = new RegisterCommand(request.Email, request.Name, request.Password, request.ConfirmPassword, request.Otp);

            var result = await sender.Send(command);

            var response = result.Adapt<RegisterResponse>();

            return Results.Ok(response);
        });
    }
}
