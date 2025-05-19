using Capstone.Application.AdminDomain.Queries.GetSystemSubjects;

namespace Capstone.API.Endpoints.AdminEndpoint
{
    public class GetSystemSubjects : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/admin/subjects", async (ISender sender, [AsParameters] PaginationRequest paginationRequest) =>
            {
                var query = new GetSystemSubjectsQuery(paginationRequest);

                var response = await sender.Send(query);

                return Results.Ok(response);
            }).RequireAuthorization(PolicyConstant.ADMIN);
        }
    }
}
