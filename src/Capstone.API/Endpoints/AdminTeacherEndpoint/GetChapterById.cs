using Capstone.Application.ChapterDomain.Queries.GetChapterById;

namespace Capstone.API.Endpoints.AdminTeacherEndpoint
{
    public class GetChapterById : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/chapters/{id}", async (ISender sender, IHttpContextAccessor httpContext, Guid id) =>
            {
                var userId = httpContext.HttpContext!.GetUserIdFromJwt();
                var role = httpContext.HttpContext!.GetUserRoleFromJwt();

                var query = new GetChapterByIdQuery(userId, role, id);

                var response = await sender.Send(query);

                return Results.Ok(response);
            }).RequireAuthorization(PolicyConstant.ADMIN_OR_TEACHER);
        }
    }
}
