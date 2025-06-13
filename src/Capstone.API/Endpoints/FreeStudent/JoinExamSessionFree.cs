using Capstone.Application.ExamSessionModule.Commands.JoinExamSessionFree;

namespace Capstone.API.Endpoints.FreeStudent
{
    public record JoinExamSessionFreeRequest(string Code, string FullName, string Email);
    public class JoinExamSessionFree : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/free/exam-sessions/{id}/join", async (ISender sender, Guid id, JoinExamSessionFreeRequest request) =>
            {
                var command = new JoinExamSessionFreeCommand(id, request.Code, request.FullName, request.Email);

                var response = await sender.Send(command);

                return Results.Ok(response);
            }).AllowAnonymous();
        }
    }
}
