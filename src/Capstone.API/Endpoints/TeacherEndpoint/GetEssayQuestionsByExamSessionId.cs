using Capstone.Application.ExamSessionModule.Queries.GetEssayQuestionsByExamSessionId;

namespace Capstone.API.Endpoints.TeacherEndpoint
{
    public class GetEssayQuestionsByExamSessionId : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/teacher/exam-sessions/{sessionId}/essay-questions", async (ISender sender, IHttpContextAccessor httpContext, Guid sessionId, [AsParameters] PaginationRequest paginationRequest) =>
            {
                var userId = httpContext.HttpContext!.GetUserIdFromJwt();

                var query = new GetEssayQuestionsByExamSessionIdQuery(userId, sessionId, paginationRequest);

                var response = await sender.Send(query);

                return Results.Ok(response);
            }).RequireAuthorization(PolicyConstant.TEACHER);
        }
    }
}
