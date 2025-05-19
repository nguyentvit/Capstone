using Capstone.Application.AdminDomain.Queries.GetSystemSubjectById;

namespace Capstone.API.Endpoints.AdminEndpoint
{
    public class GetSystemSubjectById : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/admin/subjects/{id}", async (ISender sender, Guid id) =>
            {
                var query = new GetSystemSubjectByIdQuery(id);

                var response = await sender.Send(query);

                return Results.Ok(response);
            }).RequireAuthorization(PolicyConstant.ADMIN);
        }
    }
}
