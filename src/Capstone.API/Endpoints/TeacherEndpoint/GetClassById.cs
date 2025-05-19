using Capstone.Application.TeacherDomain.Queries.GetClassById;

namespace Capstone.API.Endpoints.TeacherEndpoint
{
    public class GetClassById : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/teacher/classes/{id}", async (ISender sender, IHttpContextAccessor httpContext, Guid id) =>
            {
                var userId = httpContext.HttpContext!.GetUserIdFromJwt();

                var query = new GetClassByIdQuery(userId, id);

                var response = await sender.Send(query);

                return Results.Ok(response);
            }).RequireAuthorization(PolicyConstant.TEACHER);
        }
    }
}
