using Capstone.Application.TeacherDomain.Queries.GetClasses;

namespace Capstone.API.Endpoints.TeacherEndpoint
{
    public class GetClasses : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/teacher/classes", async (ISender sender, IHttpContextAccessor httpContext, [AsParameters] PaginationRequest paginationRequest) =>
            {
                var userId = httpContext.HttpContext!.GetUserIdFromJwt();

                var query = new GetClassesQuery(userId, paginationRequest);

                var response = await sender.Send(query);

                return Results.Ok(response);
            }).RequireAuthorization(PolicyConstant.TEACHER);
        }
    }
}
