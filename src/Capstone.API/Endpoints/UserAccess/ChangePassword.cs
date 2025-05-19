
using Capstone.Application.UserAccess.Commands.ChangePassword;

namespace Capstone.API.Endpoints.UserAccess
{
    public record ChangePasswordRequest(string Password, string ConfirmPassword, string OldPassword);
    public class ChangePassword : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/users/change-password", async (ISender sender, IHttpContextAccessor httpContext, ChangePasswordRequest request) =>
            {
                var userId = httpContext.HttpContext!.GetUserIdFromJwt();

                var command = new ChangePasswordCommand(userId, request.Password, request.ConfirmPassword, request.OldPassword);

                var response = await sender.Send(command);

                return Results.Ok(response);
            }).RequireAuthorization();
        }
    }
}
