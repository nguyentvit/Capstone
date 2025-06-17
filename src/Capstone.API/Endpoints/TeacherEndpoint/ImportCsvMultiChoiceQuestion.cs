using Capstone.Application.QuestionDomain.Commands.ImportCsvMultiChoiceQuestion;

namespace Capstone.API.Endpoints.TeacherEndpoint
{
    public record ImportCsvMultiChoiceQuestionRequest(IFormFile Csv);
    public class ImportCsvMultiChoiceQuestion : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/chapters/{chapterId}/questions/multi-choice/csv", async (ISender sender, IHttpContextAccessor httpContext, Guid chapterId, [FromForm] ImportCsvMultiChoiceQuestionRequest request) =>
            {
                var userId = httpContext.HttpContext!.GetUserIdFromJwt();

                var role = httpContext.HttpContext!.GetUserRoleFromJwt();

                var command = new ImportCsvMultiChoiceQuestionCommand(userId, role, chapterId, request.Csv);

                var response = await sender.Send(command);

                return Results.Ok(response);
            }).RequireAuthorization(PolicyConstant.TEACHER)
            .DisableAntiforgery();
        }
    }
}
