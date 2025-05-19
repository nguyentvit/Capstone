using Capstone.Application.AdminDomain.Commands.ResetPassword;

namespace Capstone.API.Endpoints.AdminEndpoint
{
    public class ResetPassword : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/admin/users/{id}/reset-password", async (ISender sender, Guid id) =>
            {
                var command = new ResetPasswordCommand(id);

                var response = await sender.Send(command);

                return Results.Ok(response);
            }).RequireAuthorization(PolicyConstant.ADMIN);
        }
    }
}
