using Capstone.Application.QuestionDomain.Commands.MarkLastVersion;

namespace Capstone.API.Endpoints.AdminTeacherEndpoint
{
    public class MarkLastVersion : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/teacher/questions/{questionId}/mark-last", async (ISender sender, IHttpContextAccessor httpContext, Guid questionId) =>
            {
                var userId = httpContext.HttpContext!.GetUserIdFromJwt();

                var command = new MarkLastVersionCommand(userId, questionId);

                var response = await sender.Send(command);

                return Results.Ok(response);
            }).RequireAuthorization(PolicyConstant.ADMIN_OR_TEACHER);
        }
    }
}
