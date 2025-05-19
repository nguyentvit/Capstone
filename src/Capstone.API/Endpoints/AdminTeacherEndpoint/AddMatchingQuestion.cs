using Capstone.Application.QuestionDomain.Commands.AddMatchingQuestion;

namespace Capstone.API.Endpoints.AdminTeacherEndpoint
{
    public record AddMatchingQuestionRequest(Guid? ChapterId, string Title, string Content, int Difficulty, Dictionary<string, string> MatchingPairs);
    public class AddMatchingQuestion : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/questions/matching-question", async (ISender sender, IHttpContextAccessor httpContext, AddMatchingQuestionRequest request) =>
            {
                var userId = httpContext.HttpContext!.GetUserIdFromJwt();
                var role = httpContext.HttpContext!.GetUserRoleFromJwt();

                var command = new AddMatchingQuestionCommand(userId, request.ChapterId, request.Title, request.Content, request.Difficulty, role, request.MatchingPairs);

                var response = await sender.Send(command);

                return Results.Ok(response);
            }).RequireAuthorization(PolicyConstant.ADMIN_OR_TEACHER);
        }
    }
}
