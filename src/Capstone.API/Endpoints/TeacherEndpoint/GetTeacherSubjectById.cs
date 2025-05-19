using Capstone.Application.TeacherDomain.Queries.GetTeacherSubjectById;

namespace Capstone.API.Endpoints.TeacherEndpoint
{
    public class GetTeacherSubjectById : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/teacher/subjects/{id}", async (ISender sender, Guid id, IHttpContextAccessor httpContext) =>
            {
                var userId = httpContext.HttpContext!.GetUserIdFromJwt();

                var query = new GetTeacherSubjectByIdQuery(userId, id);

                var response = await sender.Send(query);

                return Results.Ok(response);
            }).RequireAuthorization(PolicyConstant.TEACHER);
        }
    }
}
