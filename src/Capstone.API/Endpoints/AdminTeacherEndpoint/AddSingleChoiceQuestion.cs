using Capstone.Application.QuestionDomain.Commands.AddSingleChoiceQuestion;

namespace Capstone.API.Endpoints.AdminTeacherEndpoint
{
    public record AddSingleChoiceQuestionRequest(Guid? ChapterId, string Title, string Content, int Difficulty, List<string> Choices, int CorrectAnswerIndex);
    public class AddSingleChoiceQuestion : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/questions/single-choice", async (ISender sender, IHttpContextAccessor httpContext, AddSingleChoiceQuestionRequest request) =>
            {
                var userId = httpContext.HttpContext!.GetUserIdFromJwt();
                var role = httpContext.HttpContext!.GetUserRoleFromJwt();

                var command = new AddSingleChoiceQuestionCommand(userId, request.ChapterId, request.Title, request.Content, request.Difficulty, role, request.Choices, request.CorrectAnswerIndex);

                var response = await sender.Send(command);

                return Results.Ok(response);
            }).RequireAuthorization(PolicyConstant.ADMIN_OR_TEACHER);
        }
    }
}
