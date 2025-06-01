using Capstone.Application.QuestionDomain.Commands.AddEssayQuestion;

namespace Capstone.API.Endpoints.AdminTeacherEndpoint
{
    public record AddEssayQuestionRequest(Guid? ChapterId, string Title, string Content, int Difficulty);
    public class AddEssayQuestion : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/questions/essay-question", async (ISender sender, IHttpContextAccessor httpContext, AddEssayQuestionRequest request) =>
            {
                var userId = httpContext.HttpContext!.GetUserIdFromJwt();

                var role = httpContext.HttpContext!.GetUserRoleFromJwt();

                var command = new AddEssayQuestionCommand(userId, request.ChapterId, request.Title, request.Content, request.Difficulty, role);

                var response = await sender.Send(command);

                return Results.Ok(response);
            }).RequireAuthorization(PolicyConstant.ADMIN_OR_TEACHER);
        }
    }
}
