using Capstone.Application.StudentDomain.Commands.AnswerEssayQuestion;

namespace Capstone.API.Endpoints.StudentEndpoint
{
    public record AnswerEssayQuestionRequest(string Answer);
    public class AnswerEssayQuestion : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/student/exam-sessions/{sessionId}/questions/{questionId}/essay", async (ISender sender, IHttpContextAccessor httpContext, Guid sessionId, Guid questionId, AnswerEssayQuestionRequest request) =>
            {
                var userId = httpContext.HttpContext!.GetUserIdFromJwt();

                var command = new AnswerEssayQuestionCommand(userId, sessionId, questionId, request.Answer);

                var response = await sender.Send(command);

                return Results.Ok(response);
            }).RequireAuthorization(PolicyConstant.STUDENT);
        }
    }
}
