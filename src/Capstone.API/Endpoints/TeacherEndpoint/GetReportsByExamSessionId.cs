using Capstone.Application.ExamSessionModule.Queries.GetReportsByExamSessionId;

namespace Capstone.API.Endpoints.TeacherEndpoint
{
    public class GetReportsByExamSessionId : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/teacher/exam-sessions/{examSessionId}/reports", async (ISender sender, IHttpContextAccessor httpContext, Guid examSessionId, [FromQuery] bool? isProcess) =>
            {
                var userId = httpContext.HttpContext!.GetUserIdFromJwt();

                var query = new GetReportsByExamSessionIdQuery(userId, examSessionId, isProcess);

                var response = await sender.Send(query);

                return Results.Ok(response);
            }).RequireAuthorization(PolicyConstant.TEACHER);
        }
    }
}
