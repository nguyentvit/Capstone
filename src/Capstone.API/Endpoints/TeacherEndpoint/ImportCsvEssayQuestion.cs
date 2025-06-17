
using Capstone.Application.QuestionDomain.Commands.ImportCsvEssayQuestion;

namespace Capstone.API.Endpoints.TeacherEndpoint
{
    public record ImportCsvEssayQuestionRequest(IFormFile Csv);
    public class ImportCsvEssayQuestion : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/chapters/{chapterId}/questions/essay/csv", async (ISender sender, IHttpContextAccessor httpContext, Guid chapterId, [FromForm] ImportCsvEssayQuestionRequest request) =>
            {
                var userId = httpContext.HttpContext!.GetUserIdFromJwt();

                var role = httpContext.HttpContext!.GetUserRoleFromJwt();

                var command = new ImportCsvEssayQuestionCommand(userId, role, chapterId, request.Csv);

                var response = await sender.Send(command);

                return Results.Ok(response);
            }).RequireAuthorization(PolicyConstant.TEACHER)
            .DisableAntiforgery();
        }
    }
}
