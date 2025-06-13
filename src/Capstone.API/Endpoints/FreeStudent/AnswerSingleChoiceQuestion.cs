using Capstone.Application.FreeParticipant.Commands.AnswerSingleChoiceQuestion;

namespace Capstone.API.Endpoints.FreeStudent
{
    public record AnswerSingleChoiceQuestionRequest(Guid Answer);
    public class AnswerSingleChoiceQuestion : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/free/{participantId}/questions/{questionId}/single-choice", async (ISender sender, Guid participantId, Guid questionId, AnswerSingleChoiceQuestionRequest request) =>
            {
                var command = new AnswerSingleChoiceQuestionCommand(participantId, questionId, request.Answer);

                var response = await sender.Send(command);

                return Results.Ok(response);
            }).AllowAnonymous();
        }
    }
}
