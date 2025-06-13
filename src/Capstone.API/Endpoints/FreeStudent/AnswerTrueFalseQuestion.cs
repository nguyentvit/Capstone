using Capstone.Application.FreeParticipant.Commands.AnswerTrueFalseQuestion;

namespace Capstone.API.Endpoints.FreeStudent
{
    public record AnswerTrueFalseQuestionRequest(bool Answer);
    public class AnswerTrueFalseQuestion : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/free/{participantId}/questions/{questionId}/true-false", async (ISender sender, Guid participantId, Guid questionId, AnswerTrueFalseQuestionRequest request) =>
            {
                var command = new AnswerTrueFalseQuestionCommand(participantId, questionId, request.Answer);

                var response = await sender.Send(command);

                return Results.Ok(response);
            }).AllowAnonymous();
        }
    }
}
