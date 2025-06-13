using Capstone.Application.ExamSessionModule.Commands.VerifyCode;

namespace Capstone.API.Endpoints.FreeStudent
{
    public record VerifyCodeRequest(string Code);
    public class VerifyCode : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/free/exam-sessions/{sessionId}/verify", async (ISender sender, Guid sessionId, VerifyCodeRequest request) =>
            {
                var command = new VerifyCodeCommand(sessionId, request.Code);

                var response = await sender.Send(command);

                return Results.Ok(response);
            }).AllowAnonymous();
        }
    }
}
