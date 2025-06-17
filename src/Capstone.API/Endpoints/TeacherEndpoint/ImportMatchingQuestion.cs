
using Capstone.Application.QuestionDomain.Commands.ImportCsvMatchingQuestion;

namespace Capstone.API.Endpoints.TeacherEndpoint
{
    public record ImportMatchingQuestionRequest(IFormFile Csv);
    public class ImportMatchingQuestion : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/chapters/{chapterId}/questions/matching/csv", async (ISender sender, IHttpContextAccessor httpContext, Guid chapterId, [FromForm] ImportMatchingQuestionRequest request) =>
            {
                var userId = httpContext.HttpContext!.GetUserIdFromJwt();

                var role = httpContext.HttpContext!.GetUserRoleFromJwt();

                var command = new ImportCsvMatchingQuestionCommand(userId, role, chapterId, request.Csv);

                var response = await sender.Send(command);

                return Results.Ok(response);
            }).RequireAuthorization(PolicyConstant.TEACHER)
            .DisableAntiforgery();
        }
    }
}
