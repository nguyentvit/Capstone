using Capstone.Application.FreeParticipant.Commands.AddAction;
using Capstone.Domain.ExamSessionModule.Enums;

namespace Capstone.API.Endpoints.FreeStudent
{
    public record AddActionRequest(ActionType Action);
    public class AddAction : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/free/{participantId}/actions", async (ISender sender, Guid participantId, AddActionRequest request) =>
            {
                var command = new AddActionCommand(participantId, request.Action);

                var response = await sender.Send(command);

                return Results.Ok(response);
            }).AllowAnonymous();
        }
    }
}
