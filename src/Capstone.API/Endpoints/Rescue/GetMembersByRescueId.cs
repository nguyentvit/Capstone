
using Capstone.Application.RescueTeam.Queries.GetMembersByRescueId;

namespace Capstone.API.Endpoints.Rescue;
public class GetMembersByRescueId : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/rescues/{rescueId}/members", async (ISender sender, Guid rescueId, [AsParameters] PaginationRequest paginationRequest) => {
            var query = new GetMembersByRescueIdQuery(rescueId, paginationRequest);

            var result = await sender.Send(query);

            return Results.Ok(result);
        }).RequireAuthorization();
    }
}