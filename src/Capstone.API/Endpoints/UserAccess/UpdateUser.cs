using Capstone.Application.UserAccess.Commands.UpdateUser;

namespace Capstone.API.Endpoints.UserAccess
{
    public record UpdateUserRequest(string? UserName, string? Email, string? PhoneNumber, IFormFile? Avartar);
    public class UpdateUser : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPut("/users", async (ISender sender, IHttpContextAccessor httpContext, [FromForm] UpdateUserRequest request) =>
            {
                var userId = httpContext.HttpContext!.GetUserIdFromJwt();

                var command = new UpdateUserCommand(userId, request.UserName, request.Email, request.PhoneNumber, request.Avartar);

                var response = await sender.Send(command);

                return Results.Ok(response);
            }).RequireAuthorization();
        }
    }
}
