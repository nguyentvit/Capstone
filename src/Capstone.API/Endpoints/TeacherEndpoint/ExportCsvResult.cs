using Capstone.Application.ExamSessionModule.Commands.ExportCsvResult;

namespace Capstone.API.Endpoints.TeacherEndpoint
{
    public class ExportCsvResult : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/teacher/exam-sessions/{examSessionId}/result-pdf", async (ISender sender, IHttpContextAccessor httpContext, Guid examSessionId) =>
            {
                var userId = httpContext.HttpContext!.GetUserIdFromJwt();

                var command = new ExportCsvResultCommand(userId, examSessionId);

                var result = await sender.Send(command);

                var fileName = $"result_{examSessionId}.pdf";

                return Results.File(
                    fileContents: result.Pdf,
                    contentType: "application/pdf",
                    fileDownloadName: fileName
                    );
            }).RequireAuthorization(PolicyConstant.TEACHER);
        }
    }
}
