using Capstone.Application.ExamDomain.Commands.DownloadExamVersionPdf;

namespace Capstone.API.Endpoints.TeacherEndpoint
{
    public class DownloadExamVersionPdf : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/teacher/exam-versions/{id}/pdf", async (ISender sender, IHttpContextAccessor httpContext, Guid id) =>
            {
                var userId = httpContext.HttpContext!.GetUserIdFromJwt();

                var command = new DownloadExamVersionPdfCommand(userId, id);

                var result = await sender.Send(command);

                var fileName = $"exam_{id}.pdf";

                return Results.File(
                    fileContents: result.Pdf,
                    contentType: "application/pdf",
                    fileDownloadName: fileName
                    );
            }).RequireAuthorization(PolicyConstant.TEACHER);
        }
    }
}
