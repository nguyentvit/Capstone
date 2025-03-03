using Capstone.Application.Admin.Commands.CreateRescue;

namespace Capstone.API.Endpoints.Admin;
public record CreateRescueRequest(string RescueName, string Phone, string District, string Ward, string Province, double Longitude, double Latitude, Guid ManagerId);
public record CreateRescueResponse(Guid Id);
public class CreateRescue : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/admin/rescues", async (CreateRescueRequest request, ISender sender) => 
        {
            var command = new CreateRescueCommand(request.RescueName, request.Phone, request.District, request.Ward, request.Province, request.Longitude, request.Latitude, request.ManagerId);

            var result = await sender.Send(command);

            var response = result.Adapt<CreateRescueResponse>();

            return Results.Ok(response);
        }).RequireAuthorization("Admin");
    }
}
