using Capstone.Application.ExamSessionModule.Commands.CreateExamSession;

namespace Capstone.API.Endpoints.TeacherEndpoint
{
    public record CreateExamSessionRequest(string Name, DateTime StartTime, DateTime EndTime, int Duration, bool IsCodeBased, Guid ExamId, Guid? ClassId);
    public class CreateExamSession : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/teacher/exam-sessions", async (ISender sender, IHttpContextAccessor httpContext, CreateExamSessionRequest request) =>
            {
                var userId = httpContext.HttpContext!.GetUserIdFromJwt();

                var command = new CreateExamSessionCommand(userId, request.Name, request.StartTime, request.EndTime, request.Duration, request.IsCodeBased, request.ExamId, request.ClassId);

                var response = await sender.Send(command);

                return Results.Ok(response);
            }).RequireAuthorization(PolicyConstant.TEACHER);
        }
    }
}
