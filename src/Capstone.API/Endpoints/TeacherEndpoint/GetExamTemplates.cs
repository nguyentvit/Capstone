using Capstone.Application.ExamTemplateModule.Queries.GetExamTemplates;

namespace Capstone.API.Endpoints.TeacherEndpoint
{
    public class GetExamTemplates : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/teacher/exam-templates", async (ISender sender, IHttpContextAccessor httpContext, [AsParameters] PaginationRequest paginationRequest) =>
            {
                var userId = httpContext.HttpContext!.GetUserIdFromJwt();

                var query = new GetExamTemplatesQuery(userId, paginationRequest);

                var response = await sender.Send(query);

                return Results.Ok(response);
            }).RequireAuthorization(PolicyConstant.TEACHER);
        }
    }
}
