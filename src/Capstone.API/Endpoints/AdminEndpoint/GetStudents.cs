using Capstone.Application.AdminDomain.Queries.GetStudentsAdmin;

namespace Capstone.API.Endpoints.AdminEndpoint
{
    public class GetStudents : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("admin/students", async (ISender sender, [AsParameters] PaginationRequest paginationRequest) =>
            {
                var query = new GetStudentsAdminQuery(paginationRequest);

                var response = await sender.Send(query);

                return Results.Ok(response);

            }).RequireAuthorization(PolicyConstant.ADMIN);
        }
    }
}
