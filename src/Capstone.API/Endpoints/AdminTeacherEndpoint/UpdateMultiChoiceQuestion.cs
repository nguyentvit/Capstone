using Capstone.Application.QuestionDomain.Commands.UpdateMultiChoiceQuestion;

namespace Capstone.API.Endpoints.AdminTeacherEndpoint
{
    public record UpdateMultiChoiceQuestionRequest(string Title, string Content, int Difficulty, Dictionary<string, bool> Choices);
    public class UpdateMultiChoiceQuestion : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPut("/questions/multi-choice/{id}", async (ISender sender, IHttpContextAccessor httpContext, Guid id, UpdateMultiChoiceQuestionRequest request) =>
            {
                var userId = httpContext.HttpContext!.GetUserIdFromJwt();

                var command = new UpdateMultiChoiceQuestionCommand(userId, id, request.Title, request.Content, request.Difficulty, request.Choices);

                var response = await sender.Send(command);

                return Results.Ok(response);
            }).RequireAuthorization(PolicyConstant.ADMIN_OR_TEACHER);
        }
    }
}
