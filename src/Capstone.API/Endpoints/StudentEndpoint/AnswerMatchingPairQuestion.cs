using Capstone.Application.StudentDomain.Commands.AnswerMatchingPairQuestion;

namespace Capstone.API.Endpoints.StudentEndpoint
{
    public record AnswerMatchingPairQuestionRequest(Dictionary<Guid, Guid> Answer);
    public class AnswerMatchingPairQuestion : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/student/exam-sessions/{sessionId}/questions/{questionId}/matching-pair", async (ISender sender, IHttpContextAccessor httpContext, Guid sessionId, Guid questionId, AnswerMatchingPairQuestionRequest request) =>
            {
                var userId = httpContext.HttpContext!.GetUserIdFromJwt();

                var command = new AnswerMatchingPairQuestionCommand(userId, sessionId, questionId, request.Answer);

                var response = await sender.Send(command);

                return Results.Ok(response);
            }).RequireAuthorization(PolicyConstant.STUDENT);
        }
    }
}
