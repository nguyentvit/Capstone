using Capstone.Application.ExamTemplateModule.Queries.GetExamTemplateById;

namespace Capstone.API.Endpoints.TeacherEndpoint
{
    public class GetExamTemplateById : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/teacher/exam-template/{id}", async (ISender sender, IHttpContextAccessor httpContext, Guid id) =>
            {
                var userId = httpContext.HttpContext!.GetUserIdFromJwt();

                var query = new GetExamTemplateByIdQuery(userId, id);

                var response = await sender.Send(query);

                return Results.Ok(response);
            }).RequireAuthorization(PolicyConstant.TEACHER);
        }
    }
}
