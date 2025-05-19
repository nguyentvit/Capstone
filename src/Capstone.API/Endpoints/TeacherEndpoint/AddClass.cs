using Capstone.Application.TeacherDomain.Commands.AddClass;

namespace Capstone.API.Endpoints.TeacherEndpoint
{
    public record AddClassRequest(string ClassName);
    public class AddClass : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/teacher/subjects/{id}/classes", async (ISender sender, Guid id, AddClassRequest request, IHttpContextAccessor httpContext) =>
            {
                var userId = httpContext.HttpContext!.GetUserIdFromJwt();

                var command = new AddClassCommand(userId, id, request.ClassName);

                var response = await sender.Send(command);

                return Results.Ok(response);
            }).RequireAuthorization(PolicyConstant.TEACHER);
        }
    }
}
