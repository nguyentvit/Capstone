using Capstone.Application.ExamDomain.Commands.CreateExam;

namespace Capstone.API.Endpoints.TeacherEndpoint
{
    public record CreateExamRequest(Guid ExamTemplateId, int Duration, string Title);
    public class CreateExam : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("teacher/exams", async (ISender sender, IHttpContextAccessor httpContext, CreateExamRequest request) =>
            {
                var userId = httpContext.HttpContext!.GetUserIdFromJwt();

                var command = new CreateExamCommand(userId, request.ExamTemplateId, request.Duration, request.Title);

                var response = await sender.Send(command);

                return Results.Ok(response);
            }).RequireAuthorization(PolicyConstant.TEACHER);
        }
    }
}
