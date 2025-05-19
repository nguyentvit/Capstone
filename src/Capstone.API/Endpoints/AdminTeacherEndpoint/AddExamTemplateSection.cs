using Capstone.Application.ExamTemplateModule.Commands.AddExamTemplateSection;

namespace Capstone.API.Endpoints.AdminTeacherEndpoint
{
    public record AddExamTemplateSectionRequest(Guid ChapterId, List<AddExamTemplateSectionDifficulty> DifficultyConfigs);
    public class AddExamTemplateSection : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/exam-template/{id}/exam-template-section", async (ISender sender, IHttpContextAccessor httpContext, Guid id, AddExamTemplateSectionRequest request) =>
            {
                var userId = httpContext.HttpContext!.GetUserIdFromJwt();
                var role = httpContext.HttpContext!.GetUserRoleFromJwt();

                var command = new AddExamTemplateSectionCommand(userId, role, id, request.ChapterId, request.DifficultyConfigs);

                var response = await sender.Send(command);

                return Results.Ok(response);
            }).RequireAuthorization(PolicyConstant.ADMIN_OR_TEACHER);
        }
    }
}
