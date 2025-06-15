using Capstone.Application.FreeParticipant.Commands.SubmitExamSession;

namespace Capstone.API.Endpoints.FreeStudent
{
    public class SubmitExamSession : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/free/{participantId}/submit", async (ISender sender, Guid participantId) =>
            {
                var command = new SubmitExamSessionCommand(participantId);

                var response = await sender.Send(command);

                return Results.Ok(response);
            }).AllowAnonymous();
        }
    }
}
