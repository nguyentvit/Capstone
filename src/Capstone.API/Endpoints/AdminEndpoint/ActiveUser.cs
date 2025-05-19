
using Capstone.Application.AdminDomain.Commands.ActiveUser;

namespace Capstone.API.Endpoints.AdminEndpoint
{
    public class ActiveUser : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/admin/users/{id}/active", async (ISender sender, Guid id) =>
            {
                var command = new ActiveUserCommand(id);

                var response = await sender.Send(command);

                return Results.Ok(response);
            }).RequireAuthorization(PolicyConstant.ADMIN);
        }
    }
}
