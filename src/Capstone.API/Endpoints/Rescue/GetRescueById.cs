
using Capstone.Application.RescueTeam.Queries.GetRescueById;

namespace Capstone.API.Endpoints.Rescue;
public class GetRescueById : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/rescues/{rescueId}", async (ISender sender, Guid rescueId) => {
            var query = new GetRescueByIdQuery(rescueId);

            var result = await sender.Send(query);

            return Results.Ok(result);
        }).RequireAuthorization();
    }
}
