using Capstone.Application.ExamSessionModule.Queries.GetExamSessionById;

namespace Capstone.API.Endpoints.TeacherEndpoint
{
    public class GetExamSessionById : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/teacher/exam-sessions/{id}", async (ISender sender, IHttpContextAccessor httpContext, Guid id) =>
            {
                var userId = httpContext.HttpContext!.GetUserIdFromJwt();

                var query = new GetExamSessionByIdQuery(userId, id);

                var response = await sender.Send(query);

                return Results.Ok(response);
            }).RequireAuthorization(PolicyConstant.TEACHER);
        }
    }
}
