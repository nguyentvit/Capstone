using Capstone.Application.ExamSessionModule.Queries.GetParticipantsResult;

namespace Capstone.API.Endpoints.TeacherEndpoint
{
    public class GetParticipantsResult : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/teacher/exam-sessions/{examSessionId}/result", async (ISender sender, IHttpContextAccessor httpContext, Guid examSessionId) =>
            {
                var userId = httpContext.HttpContext!.GetUserIdFromJwt();

                var query = new GetParticipantsResultQuery(userId, examSessionId);

                var response = await sender.Send(query);

                return Results.Ok(response);
            }).RequireAuthorization(PolicyConstant.TEACHER);
        }
    }
}
