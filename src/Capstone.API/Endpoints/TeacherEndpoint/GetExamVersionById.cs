using Capstone.Application.ExamDomain.Queries.GetExamVersionById;

namespace Capstone.API.Endpoints.TeacherEndpoint
{
    public class GetExamVersionById : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("teacher/exam-versions/{id}", async (ISender sender, IHttpContextAccessor httpContext, Guid id) =>
            {
                var userId = httpContext.HttpContext!.GetUserIdFromJwt();

                var query = new GetExamVersionByIdQuery(userId, id);

                var response = await sender.Send(query);

                return Results.Ok(response);
            }).RequireAuthorization(PolicyConstant.TEACHER);
        }
    }
}
