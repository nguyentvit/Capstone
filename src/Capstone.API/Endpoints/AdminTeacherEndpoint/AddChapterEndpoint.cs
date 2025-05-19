using Capstone.Application.ChapterDomain.Commands.AddChapter;

namespace Capstone.API.Endpoints.AdminTeacherEndpoint
{
    public record AddChapterRequest(string Title);
    public class AddChapterEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/subjects/{id}/chapters", async (ISender sender, IHttpContextAccessor httpContext, AddChapterRequest request, Guid id) =>
            {
                var userId = httpContext.HttpContext!.GetUserIdFromJwt();
                var role = httpContext.HttpContext!.GetUserRoleFromJwt();

                var command = new AddChapterCommand(userId, role, id, request.Title);

                var response = await sender.Send(command);

                return Results.Ok(response);
            }).RequireAuthorization(PolicyConstant.ADMIN_OR_TEACHER);
        }
    }
}
