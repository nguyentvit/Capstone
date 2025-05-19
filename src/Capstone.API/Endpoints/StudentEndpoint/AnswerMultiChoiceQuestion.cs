using Capstone.Application.StudentDomain.Commands.AnswerMultiChoiceQuestion;

namespace Capstone.API.Endpoints.StudentEndpoint
{
    public record AnswerMultiChoiceQuestionRequest(List<Guid> Answer);
    public class AnswerMultiChoiceQuestion : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/student/exam-sessions/{sessionId}/questions/{questionId}/multi-choice", async (ISender sender, IHttpContextAccessor httpContext, Guid sessionId, Guid questionId, AnswerMultiChoiceQuestionRequest request) =>
            {
                var userId = httpContext.HttpContext!.GetUserIdFromJwt();

                var command = new AnswerMultiChoiceQuestionCommand(userId, sessionId, questionId, request.Answer);

                var response = await sender.Send(command);

                return Results.Ok(response);
            }).RequireAuthorization(PolicyConstant.STUDENT);
        }
    }
}
