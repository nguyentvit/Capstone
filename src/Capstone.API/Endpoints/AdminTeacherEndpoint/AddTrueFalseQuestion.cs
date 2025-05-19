using Capstone.Application.QuestionDomain.Commands.AddTrueFalseQuestion;

namespace Capstone.API.Endpoints.AdminTeacherEndpoint
{
    public record AddTrueFalseQuestionRequest(Guid? ChapterId, string Title, string Content, bool IsTrueAnswer, int Difficulty);
    public class AddTrueFalseQuestion : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/questions/true-false", async (ISender sender, IHttpContextAccessor httpContext, AddTrueFalseQuestionRequest request) =>
            {
                var userId = httpContext.HttpContext!.GetUserIdFromJwt();

                var role = httpContext.HttpContext!.GetUserRoleFromJwt();

                var command = new AddTrueFalseQuestionCommand(userId, request.ChapterId, request.Title, request.Content, request.IsTrueAnswer, request.Difficulty, role);

                var response = await sender.Send(command);

                return Results.Ok(response);
            }).RequireAuthorization(PolicyConstant.ADMIN_OR_TEACHER);
        }
    }
}
