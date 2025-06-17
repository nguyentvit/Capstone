using Capstone.Application.StudentDomain.Commands.AddReport;

namespace Capstone.API.Endpoints.StudentEndpoint
{
    public class AddReport : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/student/exam-sessions/{examSessionId}/questions/{questionId}/report", async (ISender sender, IHttpContextAccessor httpContext, Guid examSessionId, Guid questionId) =>
            {
                var userId = httpContext.HttpContext!.GetUserIdFromJwt();

                var command = new AddReportCommand(userId, examSessionId, questionId);

                var response = await sender.Send(command);

                return Results.Ok(response);
            }).RequireAuthorization(PolicyConstant.STUDENT);
        }
    }
}
