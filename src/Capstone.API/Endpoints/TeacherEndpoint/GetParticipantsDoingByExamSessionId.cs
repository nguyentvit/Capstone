using Capstone.Application.ExamSessionModule.Queries.GetParticipantsDoingByExamSessionId;

namespace Capstone.API.Endpoints.TeacherEndpoint
{
    public class GetParticipantsDoingByExamSessionId : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/teacher/exam-sessions/{id}/doing", async (ISender sender, IHttpContextAccessor httpContext, Guid id) =>
            {
                var userId = httpContext.HttpContext!.GetUserIdFromJwt();

                var query = new GetParticipantsDoingByExamSessionIdQuery(userId, id);

                var response = await sender.Send(query);

                return Results.Ok(response);
            }).RequireAuthorization(PolicyConstant.TEACHER);
        }
    }
}
