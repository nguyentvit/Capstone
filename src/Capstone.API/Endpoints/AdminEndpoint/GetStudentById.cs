
using Capstone.Application.AdminDomain.Queries.GetStudentById;

namespace Capstone.API.Endpoints.AdminEndpoint
{
    public class GetStudentById : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/admin/students/{id}", async (ISender sender, Guid id) =>
            {
                var query = new GetStudentByIdQuery(id);

                var response = await sender.Send(query);

                return Results.Ok(response);
            }).RequireAuthorization(PolicyConstant.ADMIN);
        }
    }
}
