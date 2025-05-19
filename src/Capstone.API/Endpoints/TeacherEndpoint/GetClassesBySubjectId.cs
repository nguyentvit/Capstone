using Capstone.Application.TeacherDomain.Queries.GetClassesBySubjectId;

namespace Capstone.API.Endpoints.TeacherEndpoint
{
    public class GetClassesBySubjectId : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/teacher/subjects/{id}/classes", async (ISender sender, Guid id, IHttpContextAccessor httpContext, [AsParameters] PaginationRequest paginationRequest) =>
            {
                var userId = httpContext.HttpContext!.GetUserIdFromJwt();

                var query = new GetClassesBySubjectIdQuery(userId, id, paginationRequest);

                var response = await sender.Send(query);

                return Results.Ok(response);
            }).RequireAuthorization(PolicyConstant.TEACHER);
        }
    }
}
