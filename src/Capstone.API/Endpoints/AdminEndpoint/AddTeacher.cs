using Capstone.Application.AdminDomain.Commands.AddTeacher;

namespace Capstone.API.Endpoints.AdminEndpoint
{
    public class AddTeacherRequest
    {
        public string UserName { get; set; } = default!;
        public string TeacherId { get; set; } = default!;
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public IFormFile? Avatar { get; set; }
    };
    public class AddTeacher : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/admin/teachers", async (ISender sender,[FromForm] AddTeacherRequest request) =>
            {
                var command = new AddTeacherCommand(request.UserName, request.TeacherId, request.Email, request.PhoneNumber, request.Avatar);
                
                var response = await sender.Send(command);

                return Results.Ok(response);
            })
                .RequireAuthorization(PolicyConstant.ADMIN)
                .DisableAntiforgery();
        }
    }
}
