
using Capstone.Application.ExamDomain.Queries.GetExamsBySubjectId;

namespace Capstone.API.Endpoints.TeacherEndpoint
{
    public class GetExamsBySubjectId : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/teacher/subjects/{id}/exams", async (ISender sender, IHttpContextAccessor httpContext, Guid id, [AsParameters] PaginationRequest paginationRequest) =>
            {
                var userId = httpContext.HttpContext!.GetUserIdFromJwt();

                var query = new GetExamsBySubjectIdQuery(userId, id, paginationRequest);

                var response = await sender.Send(query);

                return Results.Ok(response);
            }).RequireAuthorization(PolicyConstant.TEACHER);
        }
    }
}
