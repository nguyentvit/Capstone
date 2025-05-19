using Capstone.Application.ExamTemplateModule.Commands.CreateExamTemplate;

namespace Capstone.API.Endpoints.AdminTeacherEndpoint
{
    public record CreateExamTemplateRequest(Guid SubjectId, string Title, string Description, int Duration);
    public class CreateExamTemplate : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/exam-template", async (ISender sender, IHttpContextAccessor httpContext, CreateExamTemplateRequest request) =>
            {
                var userId = httpContext.HttpContext!.GetUserIdFromJwt();
                var role = httpContext.HttpContext!.GetUserRoleFromJwt();

                var command = new CreateExamTemplateCommand(userId, role, request.SubjectId, request.Title, request.Description, request.Duration);

                var response = await sender.Send(command);

                return Results.Ok(response);
            }).RequireAuthorization(PolicyConstant.ADMIN_OR_TEACHER);
        }
    }
}
