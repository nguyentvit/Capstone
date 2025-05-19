using Capstone.Application.TeacherDomain.Commands.AddStudent;

namespace Capstone.API.Endpoints.TeacherEndpoint
{
    public record AddStudentToClassRequest(Guid StudentId);
    public class AddStudentToClass : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/teacher/classes/{id}/students", async (ISender sender, IHttpContextAccessor httpContext, Guid id, AddStudentToClassRequest request) =>
            {
                var userId = httpContext.HttpContext!.GetUserIdFromJwt();

                var command = new AddStudentCommand(userId, id, request.StudentId);

                var response = await sender.Send(command);

                return Results.Ok(response);
            }).RequireAuthorization(PolicyConstant.TEACHER);
        }
    }
}
