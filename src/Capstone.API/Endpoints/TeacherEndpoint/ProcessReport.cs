using Capstone.Application.ExamSessionModule.Commands.ProcessReport;

namespace Capstone.API.Endpoints.TeacherEndpoint
{
    public record ProcessReportRequest(double Score);
    public class ProcessReport : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/teacher/participants/{participantId}/questions/{questionId}/process", async (ISender sender, Guid participantId, Guid questionId, ProcessReportRequest request) =>
            {
                var command = new ProcessReportCommand(participantId, questionId, request.Score);

                var response = await sender.Send(command);

                return Results.Ok(response);
            }).RequireAuthorization(PolicyConstant.TEACHER);
        }
    }
}
