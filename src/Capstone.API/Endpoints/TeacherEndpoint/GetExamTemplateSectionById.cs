using Capstone.Application.ExamTemplateModule.Queries.GetExamTemplateSectionById;

namespace Capstone.API.Endpoints.TeacherEndpoint
{
    public class GetExamTemplateSectionById : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/teacher/exam-template-sections/{id}", async (ISender sender, IHttpContextAccessor httpContext, Guid id) =>
            {
                var userId = httpContext.HttpContext!.GetUserIdFromJwt();

                var query = new GetExamTemplateSectionByIdQuery(userId, id);

                var response = await sender.Send(query);

                return Results.Ok(response);
            }).RequireAuthorization(PolicyConstant.TEACHER);
        }
    }
}
