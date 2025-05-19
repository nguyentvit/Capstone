using Capstone.Application.StudentDomain.Queries.GetExamSessions;

namespace Capstone.API.Endpoints.StudentEndpoint
{
    public class GetExamSessions : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/student/exam-sessions", async (ISender sender, IHttpContextAccessor httpContext, [AsParameters] PaginationRequest paginationRequest) =>
            {
                var userId = httpContext.HttpContext!.GetUserIdFromJwt();

                var query = new GetExamSessionsQuery(userId, paginationRequest);

                var response = await sender.Send(query);

                return Results.Ok(response);
            }).RequireAuthorization(PolicyConstant.STUDENT);
        }
    }
}
