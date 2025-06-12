using Capstone.Application.ExamSessionModule.Queries.GetParticipantActionByExamSessionId;

namespace Capstone.API.Endpoints.TeacherEndpoint
{
    public class GetParticipantActionsByExamSessionId : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/teacher/exam-sessions/{id}/actions", async (ISender sender, IHttpContextAccessor httpContext, Guid id, [FromQuery] GetParticipantActionByExamSessionIdCondition condition = GetParticipantActionByExamSessionIdCondition.All) =>
            {
                var userId = httpContext.HttpContext!.GetUserIdFromJwt();

                var query = new GetParticipantActionByExamSessionIdQuery(userId, id, condition);

                var response = await sender.Send(query);

                return Results.Ok(response);
            }).RequireAuthorization(PolicyConstant.TEACHER);
        }
    }
}
