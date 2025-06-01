using Capstone.Application.ExamDomain.Queries.GetExams;

namespace Capstone.API.Endpoints.TeacherEndpoint
{
    public class GetExams : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/teacher/exams", async (ISender sender, IHttpContextAccessor httpContext, [AsParameters] PaginationRequest paginationRequest) =>
            {
                var userId = httpContext.HttpContext!.GetUserIdFromJwt();

                var query = new GetExamsQuery(userId, paginationRequest);

                var response = await sender.Send(query);

                return Results.Ok(response);
            }).RequireAuthorization(PolicyConstant.TEACHER);
        }
    }
}
