using Capstone.Application.QuestionDomain.Commands.UpdateSingleChoiceQuestion;

namespace Capstone.API.Endpoints.AdminTeacherEndpoint
{
    public record UpdateSingleChoiceQuestionRequest(string Title, string Content, int Difficulty, List<string> Choices, int CorrectAnswerIndex);
    public class UpdateSingleChoiceQuestion : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPut("/questions/single-choice/{id}", async (ISender sender, IHttpContextAccessor httpContext, Guid id, UpdateSingleChoiceQuestionRequest request) =>
            {
                var userId = httpContext.HttpContext!.GetUserIdFromJwt();

                var command = new UpdateSingleChoiceQuestionCommand(userId, id, request.Title, request.Content, request.Difficulty, request.Choices, request.CorrectAnswerIndex);

                var response = await sender.Send(command);

                return Results.Ok(response);
            }).RequireAuthorization(PolicyConstant.ADMIN_OR_TEACHER);
        }
    }
}
