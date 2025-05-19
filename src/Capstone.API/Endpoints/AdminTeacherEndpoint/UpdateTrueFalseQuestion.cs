using Capstone.Application.QuestionDomain.Commands.UpdateTrueFalseQuestion;

namespace Capstone.API.Endpoints.AdminTeacherEndpoint
{
    public record UpdateTrueFalseQuestionRequest(string Title, string Content, bool IsTrueAnswer, int Difficulty);
    public class UpdateTrueFalseQuestion : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPut("/questions/true-false/{id}", async (ISender sender, IHttpContextAccessor httpContext, Guid id, UpdateTrueFalseQuestionRequest request) =>
            {
                var userId = httpContext.HttpContext!.GetUserIdFromJwt();

                var command = new UpdateTrueFalseQuestionCommand(userId, id, request.Title, request.Content, request.IsTrueAnswer, request.Difficulty);

                var response = await sender.Send(command);

                return Results.Ok(response);
            }).RequireAuthorization(PolicyConstant.ADMIN_OR_TEACHER);
        }
    }
}
