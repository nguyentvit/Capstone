using Capstone.Application.QuestionDomain.Commands.UpdateMatchingQuestion;

namespace Capstone.API.Endpoints.AdminTeacherEndpoint
{
    public record UpdateMatchingQuestionRequest(string Title, string Content, int Difficulty, Dictionary<string, string> MatchingPairs);
    public class UpdateMatchingQuestion : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPut("/questions/matching-question/{id}", async (ISender sender, IHttpContextAccessor httpContext, Guid id, UpdateMatchingQuestionRequest request) =>
            {
                var userId = httpContext.HttpContext!.GetUserIdFromJwt();

                var command = new UpdateMatchingQuestionCommand(userId, id, request.Title, request.Content, request.Difficulty, request.MatchingPairs);

                var response = await sender.Send(command);

                return Results.Ok(response);
            }).RequireAuthorization(PolicyConstant.ADMIN_OR_TEACHER);
        }
    }
}
