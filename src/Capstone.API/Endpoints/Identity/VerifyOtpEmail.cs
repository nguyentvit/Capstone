using Capstone.Application.Identity.Commands.VerifyOtpEmail;

namespace Capstone.API.Endpoints.Identity;
public record VerifyOtpEmailRequest(string Email, string Otp);
public record VerifyOtpEmailResponse(bool IsSuccess);
public class VerifyOtpEmail : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/identity/verify-email", async (ISender sender, VerifyOtpEmailRequest request) => 
        {
            var command = new VerifyOtpEmailCommand(request.Email, request.Otp);

            var result = await sender.Send(command);

            var response = result.Adapt<VerifyOtpEmailResponse>();

            return Results.Ok(response);
        });
    }
}
