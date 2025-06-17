using Capstone.Application.QuestionDomain.Commands.ImportCsvSingleChoiceQuestion;

namespace Capstone.API.Endpoints.TeacherEndpoint
{
    public record ImportCsvSingleChoiceQuestionRequest(IFormFile Csv);
    public class ImportCsvSingleChoiceQuestion : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/chapters/{chapterId}/questions/single-choice/csv", async (ISender sender, IHttpContextAccessor httpContext, Guid chapterId, [FromForm] ImportCsvSingleChoiceQuestionRequest request) =>
            {
                var userId = httpContext.HttpContext!.GetUserIdFromJwt();

                var role = httpContext.HttpContext!.GetUserRoleFromJwt();

                var command = new ImportCsvSingleChoiceQuestionCommand(userId, role, chapterId, request.Csv);

                var response = await sender.Send(command);

                return Results.Ok(response);
            }).RequireAuthorization(PolicyConstant.TEACHER)
            .DisableAntiforgery();
        }
    }
}
