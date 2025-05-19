using Capstone.Application.AdminDomain.Commands.AddStudent;

namespace Capstone.API.Endpoints.AdminEndpoint
{
    public class AddStudentRequest 
    {
        public string UserName { get; set; } = default!;
        public string StudentId { get; set; } = default!;
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public IFormFile? Avatar { get; set; }
    };
    public class AddStudent : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/admin/students", async (ISender sender, [FromForm] AddStudentRequest request) =>
            {
                var command = new AddStudentCommand(request.UserName, request.StudentId, request.Email, request.PhoneNumber, request.Avatar);

                var response = await sender.Send(command);

                return Results.Ok(response);
            })
                .RequireAuthorization(PolicyConstant.ADMIN)
                .DisableAntiforgery();
        }
    }
}
