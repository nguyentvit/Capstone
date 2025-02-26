using Capstone.Application.Identity.Commands.SendOtp;

namespace Capstone.API.Endpoints.Identity;
public record SendOtpRequest(string Email);
public record SendOtpResponse(bool IsSuccess);
public class SendOtp : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/identity/send-otp", async (ISender sender, SendOtpRequest request) => {
            
            var command = new SendOtpCommand(request.Email);

            var result = await sender.Send(command);

            var response = result.Adapt<SendOtpResponse>();

            return Results.Ok(response);
        });
    }
}