using Capstone.Application.ExamSessionModule.Queries.GetParticipantsByExamSessionId;

namespace Capstone.API.Endpoints.TeacherEndpoint
{
    public class GetParticipantsByExamSessionId : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/teacher/exam-sessions/{id}/participants", async (ISender sender, IHttpContextAccessor httpContext, Guid id, [AsParameters] PaginationRequest paginationRequest, [FromQuery] bool IsFree = false) =>
            {
                var userId = httpContext.HttpContext!.GetUserIdFromJwt();

                var query = new GetParticipantsByExamSessionIdQuery(userId, id, IsFree, paginationRequest);

                var response = await sender.Send(query);

                return Results.Ok(response);
            }).RequireAuthorization(PolicyConstant.TEACHER);
        }
    }
}
