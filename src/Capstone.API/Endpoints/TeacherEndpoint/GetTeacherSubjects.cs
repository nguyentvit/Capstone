using Capstone.Application.TeacherDomain.Queries.GetTeacherSubjects;

namespace Capstone.API.Endpoints.TeacherEndpoint
{
    public class GetTeacherSubjects : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/teacher/subjects", async (ISender sender, IHttpContextAccessor httpContext, [AsParameters] PaginationRequest paginationRequest) =>
            {
                var userId = httpContext.HttpContext!.GetUserIdFromJwt();

                var query = new GetTeacherSubjectsQuery(userId, paginationRequest);

                var response = await sender.Send(query);

                return Results.Ok(response);
            }).RequireAuthorization(PolicyConstant.TEACHER);
        }
    }
}
