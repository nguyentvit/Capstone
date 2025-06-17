using Capstone.Application.ExamSessionModule.Commands.ClosePoint;

namespace Capstone.API.Endpoints.TeacherEndpoint
{
    public class ClosePoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/teacher/exam-sessions/{examSessionId}/close-point", async (ISender sender, IHttpContextAccessor httpContext, Guid examSessionId) =>
            {
                var userId = httpContext.HttpContext!.GetUserIdFromJwt();

                var command = new ClosePointCommand(userId, examSessionId);

                var response = await sender.Send(command);

                return Results.Ok(response);
            }).RequireAuthorization(PolicyConstant.TEACHER);
        }
    }
}
