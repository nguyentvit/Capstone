using Capstone.Application.QuestionDomain.Commands.AddMultiChoiceQuestion;

namespace Capstone.API.Endpoints.AdminTeacherEndpoint
{
    public record AddMultiChoiceQuestionRequest(Guid? ChapterId, string Title, string Content, int Difficulty, Dictionary<string, bool> Choices);
    public class AddMultiChoiceQuestion : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/questions/multi-choice", async (ISender sender, IHttpContextAccessor httpConteext, AddMultiChoiceQuestionRequest request) =>
            {
                var userId = httpConteext.HttpContext!.GetUserIdFromJwt();
                var role = httpConteext.HttpContext!.GetUserRoleFromJwt();

                var command = new AddMultiChoiceQuestionCommand(userId, request.ChapterId, request.Title, request.Content, request.Difficulty, role, request.Choices);

                var response = await sender.Send(command);

                return Results.Ok(response);
            }).RequireAuthorization(PolicyConstant.ADMIN_OR_TEACHER);
        }
    }
}
