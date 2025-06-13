using Capstone.Application.FreeParticipant.Commands.AnswerMultiChoiceQuestion;

namespace Capstone.API.Endpoints.FreeStudent
{
    public record AnswerMultiChoiceQuestionRequest(List<Guid> Answer);
    public class AnswerMultiChoiceQuestion : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/free/{participantId}/questions/{questionId}/multi-choice", async (ISender sender, IHttpContextAccessor httpContext, Guid participantId, Guid questionId, AnswerMultiChoiceQuestionRequest request) =>
            {
                var command = new AnswerMultiChoiceQuestionCommand(participantId, questionId, request.Answer);

                var response = await sender.Send(command);

                return Results.Ok(response);
            }).AllowAnonymous();
        }
    }
}
