using Capstone.Application.FreeParticipant.Commands.AnswerMatchingPairQuestion;

namespace Capstone.API.Endpoints.FreeStudent
{
    public record AnswerMatchingPairQuestionRequest(Dictionary<Guid, Guid> Answer);
    public class AnswerMatchingPairQuestion : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/free/{participantId}/questions/{questionId}/matching-pair", async (ISender sender, Guid participantId, Guid questionId, AnswerMatchingPairQuestionRequest request) =>
            {
                var command = new AnswerMatchingPairQuestionCommand(participantId, questionId, request.Answer);

                var response = await sender.Send(command);

                return Results.Ok(response);
            }).AllowAnonymous();
        }
    }
}
