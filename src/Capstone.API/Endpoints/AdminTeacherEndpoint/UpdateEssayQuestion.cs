using Capstone.Application.QuestionDomain.Commands.UpdateEssayQuestion;

namespace Capstone.API.Endpoints.AdminTeacherEndpoint
{
    public record UpdateEssayQuestionRequest(string Title, string Content, int Difficulty);
    public class UpdateEssayQuestion : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPut("/questions/essay-question/{id}", async (ISender sender, IHttpContextAccessor httpContext, Guid id, UpdateEssayQuestionRequest request) =>
            {
                var userId = httpContext.HttpContext!.GetUserIdFromJwt();

                var command = new UpdateEssayQuestionCommand(userId, id, request.Title, request.Content, request.Difficulty);

                var response = await sender.Send(command);

                return Results.Ok(response);
            }).RequireAuthorization(PolicyConstant.ADMIN_OR_TEACHER);
        }
    }
}
