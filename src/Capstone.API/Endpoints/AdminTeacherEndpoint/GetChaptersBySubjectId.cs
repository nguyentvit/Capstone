using Capstone.Application.ChapterDomain.Queries.GetChaptersBySubjectId;

namespace Capstone.API.Endpoints.AdminTeacherEndpoint
{
    public class GetChaptersBySubjectId : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/subjects/{id}/chapters", async (ISender sender, IHttpContextAccessor httpContext, Guid id, [AsParameters] PaginationRequest paginationRequest) =>
            {
                var userId = httpContext.HttpContext!.GetUserIdFromJwt();
                var role = httpContext.HttpContext!.GetUserRoleFromJwt();

                var query = new GetChaptersBySubjectIdQuery(userId, role, id, paginationRequest);

                var response = await sender.Send(query);

                return Results.Ok(response);
            }).RequireAuthorization(PolicyConstant.ADMIN_OR_TEACHER);
        }
    }
}
