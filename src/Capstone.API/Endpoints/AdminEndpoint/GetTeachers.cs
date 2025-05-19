using Capstone.Application.AdminDomain.Queries.GetTeachers;

namespace Capstone.API.Endpoints.AdminEndpoint
{
    public class GetTeachers : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("admin/teachers", async (ISender sender,[AsParameters] PaginationRequest paginationRequest) =>
            {
                var query = new GetTeachersQuery(paginationRequest);

                var response = await sender.Send(query);

                return Results.Ok(response);

            }).RequireAuthorization(PolicyConstant.ADMIN);
        }
    }
}
