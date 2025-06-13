using Capstone.Application.FreeParticipant.Commands.AnswerEssayQuestion;

namespace Capstone.API.Endpoints.FreeStudent
{
    public record AnswerEssayQuestionRequest(string Answer);
    public class AnswerEssayQuestion : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/free/{participantId}/questions/{questionId}/essay", async (ISender sender, Guid participantId, Guid questionId, AnswerEssayQuestionRequest request) =>
            {
                var command = new AnswerEssayQuestionCommand(participantId, questionId, request.Answer);

                var response = await sender.Send(command);

                return Results.Ok(response);
            }).AllowAnonymous();
        }
    }
}
