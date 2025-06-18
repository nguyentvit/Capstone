using Capstone.Application.ExamSessionModule.Commands.CloseExamSession;

namespace Capstone.API.Endpoints.TeacherEndpoint
{
    public class CloseExamSession : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/teacher/exam-sessions/{examSessionId}/done", async (ISender sender, IHttpContextAccessor httpContext, Guid examSessionId) =>
            {
                var userId = httpContext.HttpContext!.GetUserIdFromJwt();

                var command = new CloseExamSessionCommand(userId, examSessionId);

                var response = await sender.Send(command);

                return Results.Ok(response);
            }).RequireAuthorization(PolicyConstant.TEACHER);
        }
    }
}
