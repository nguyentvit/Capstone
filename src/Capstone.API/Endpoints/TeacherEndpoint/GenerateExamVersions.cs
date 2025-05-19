using Capstone.Application.ExamDomain.Commands.GenerateExamVersions;

namespace Capstone.API.Endpoints.TeacherEndpoint
{
    public record GenerateExamVersionsRequest(int Count, int OrderQuestion, bool IsAnswerShuffled);
    public class GenerateExamVersions : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/teacher/exams/{id}/generate", async (ISender sender, IHttpContextAccessor httpContext, Guid id, GenerateExamVersionsRequest request) =>
            {
                var userId = httpContext.HttpContext!.GetUserIdFromJwt();

                var command = new GenerateExamVersionsCommand(userId, id, request.Count, request.OrderQuestion, request.IsAnswerShuffled);

                var response = await sender.Send(command);

                return Results.Ok(response);
            }).RequireAuthorization(PolicyConstant.TEACHER);
        }
    }
}
