using Capstone.Application.TeacherDomain.Queries.GetStudentsByClassId;

namespace Capstone.API.Endpoints.TeacherEndpoint
{
    public class GetStudentsByClassId : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/teacher/classes/{id}/students", async (ISender sender, IHttpContextAccessor httpContext, Guid id, [AsParameters] PaginationRequest paginationRequest) =>
            {
                var userId = httpContext.HttpContext!.GetUserIdFromJwt();

                var query = new GetStudentsByClassIdQuery(userId, id, paginationRequest);

                var response = await sender.Send(query);

                return Results.Ok(response);
            }).RequireAuthorization(PolicyConstant.TEACHER);
        }
    }
}
