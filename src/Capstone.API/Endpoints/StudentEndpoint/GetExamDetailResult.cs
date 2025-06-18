using Capstone.Application.StudentDomain.Queries.GetDetailExamResult;

namespace Capstone.API.Endpoints.StudentEndpoint
{
    public class GetExamDetailResult : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/student/exam-sessions/{examSessionId}/result", async (ISender sender, IHttpContextAccessor httpContext, Guid examSessionId) =>
            {
                var userId = httpContext.HttpContext!.GetUserIdFromJwt();

                var query = new GetDetailExamResultQuery(userId, examSessionId);

                var response = await sender.Send(query);

                return Results.Ok(response);
            }).RequireAuthorization(PolicyConstant.STUDENT);
        }
    }
}
