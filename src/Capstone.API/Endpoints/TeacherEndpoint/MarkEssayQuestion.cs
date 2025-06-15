using Capstone.Application.ExamSessionModule.Commands.MarkEssayQuestion;

namespace Capstone.API.Endpoints.TeacherEndpoint
{
    public record MarkEssayQuestionRequest(double Score);
    public class MarkEssayQuestion : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/teacher/participants/{participantId}/questions/{questionId}", async (ISender sender, IHttpContextAccessor httpContext, Guid participantId, Guid questionId, MarkEssayQuestionRequest request) =>
            {
                var userId = httpContext.HttpContext!.GetUserIdFromJwt();

                var command = new MarkEssayQuestionCommand(userId, participantId, questionId, request.Score);

                var response = await sender.Send(command);

                return Results.Ok(response);
            }).RequireAuthorization(PolicyConstant.TEACHER);
        }
    }
}
