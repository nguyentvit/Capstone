    using Capstone.Application.TeacherDomain.Commands.AddTeacherSubject;

namespace Capstone.API.Endpoints.TeacherEndpoint
{
    public record AddTeacherSubjectRequest(string SubjectName);
    public class AddTeacherSubject : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/teacher/subjects", async (ISender sender, AddTeacherSubjectRequest request, IHttpContextAccessor httpContext) =>
            {
                var userId = httpContext.HttpContext!.GetUserIdFromJwt();

                var command = new AddTeacherSubjectCommand(userId, request.SubjectName);

                var response = await sender.Send(command);

                return Results.Ok(response);
            }).RequireAuthorization(PolicyConstant.TEACHER);
        }
    }
}
