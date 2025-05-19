using Capstone.Application.ExamTemplateModule.Queries.GetExamTemplatesBySubjectId;

namespace Capstone.API.Endpoints.TeacherEndpoint
{
    public class GetExamTemplatesBySubjectId : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("teacher/subjects/{subjectId}/exam-templates", async (ISender sender, IHttpContextAccessor httpContext, Guid subjectId, [AsParameters] PaginationRequest paginationRequest) =>
            {
                var userId = httpContext.HttpContext!.GetUserIdFromJwt();

                var query = new GetExamTemplatesBySubjectIdQuery(userId, subjectId, paginationRequest);

                var response = await sender.Send(query);

                return Results.Ok(response);
            }).RequireAuthorization(PolicyConstant.TEACHER);
        }
    }
}
