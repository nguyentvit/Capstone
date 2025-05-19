using Capstone.Application.AdminDomain.Commands.DeactiveUser;

namespace Capstone.API.Endpoints.AdminEndpoint
{
    public class DeactiveUser : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/admin/users/{id}/deactive", async (ISender sender, Guid id) =>
            {
                var command = new DeactiveUserCommand(id);

                var response = await sender.Send(command);

                return Results.Ok(response);
            }).RequireAuthorization(PolicyConstant.ADMIN);
        }
    }
}
