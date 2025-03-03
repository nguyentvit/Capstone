
using Capstone.Application.Admin.Queries.GetRescues;

namespace Capstone.API.Endpoints.Admin;
public class GetRescues : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/admin/rescues", async (ISender sender,[AsParameters] PaginationRequest paginationRequest) => {
            var query = new GetRescuesQuery(paginationRequest);

            var result = await sender.Send(query);

            return Results.Ok(result);
        }).RequireAuthorization("Admin");
    }       
}
