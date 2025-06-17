using Capstone.Application.QuestionDomain.Commands.ImportCsvTrueFalseQuestion;

namespace Capstone.API.Endpoints.TeacherEndpoint
{
    public record ImportCsvTrueFalseQuestionRequest(IFormFile Csv);
    public class ImportCsvTrueFalseQuestion : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/chapters/{chapterId}/questions/true-false/csv", async (ISender sender, IHttpContextAccessor httpContext, Guid chapterId, [FromForm] ImportCsvTrueFalseQuestionRequest request) =>
            {
                var userId = httpContext.HttpContext!.GetUserIdFromJwt();

                var role = httpContext.HttpContext!.GetUserRoleFromJwt();

                var command = new ImportCsvTrueFalseQuestionCommand(userId, role, chapterId, request.Csv);

                var response = await sender.Send(command);

                return Results.Ok(response);
            }).RequireAuthorization(PolicyConstant.TEACHER)
            .DisableAntiforgery();
        }
    }
}
