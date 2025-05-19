using Capstone.Application.ExamTemplateModule.Queries.GetExamTemplateSectionsByExamTemplateId;

namespace Capstone.API.Endpoints.TeacherEndpoint
{
    public class GetExamTemplateSectionsByExamTemplateId : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/teacher/exam-template/{id}/exam-template-sections", async (ISender sender, IHttpContextAccessor httpContext, Guid id) =>
            {
                var userId = httpContext.HttpContext!.GetUserIdFromJwt();

                var query = new GetExamTemplateSectionsByExamTemplateIdQuery(userId, id);

                var response = await sender.Send(query);

                return Results.Ok(response);
            }).RequireAuthorization(PolicyConstant.TEACHER);
        }
    }
}
