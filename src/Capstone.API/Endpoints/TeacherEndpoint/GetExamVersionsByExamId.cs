using Capstone.Application.ExamDomain.Queries.GetExamVersionsByExamId;

namespace Capstone.API.Endpoints.TeacherEndpoint
{
    public class GetExamVersionsByExamId : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/teacher/exams/{id}/exam-versions", async (ISender sender, IHttpContextAccessor httpContext, Guid id) =>
            {
                var userId = httpContext.HttpContext!.GetUserIdFromJwt();

                var query = new GetExamVersionsByExamIdQuery(userId, id);

                var response = await sender.Send(query);

                return Results.Ok(response);
            }).RequireAuthorization(PolicyConstant.TEACHER);
        }
    }
}
