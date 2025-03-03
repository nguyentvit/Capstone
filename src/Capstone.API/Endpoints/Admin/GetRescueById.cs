
namespace Capstone.API.Endpoints.Admin;
public class GetRescueById : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/admin/rescues/{rescueId}", (ISender sender, Guid rescueId) => {

        });
    }
}
