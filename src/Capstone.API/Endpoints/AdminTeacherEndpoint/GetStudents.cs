using Capstone.Application.StudentDomain.Queries.GetStudents;

namespace Capstone.API.Endpoints.AdminTeacherEndpoint
{
    public class GetStudents : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/students", async (ISender sender, [AsParameters] PaginationRequest paginationRequest, [FromQuery] string studentId = "") =>
            {
                var query = new GetStudentsQuery(studentId, paginationRequest);

                var response = await sender.Send(query);

                return Results.Ok(response);
            }).RequireAuthorization(PolicyConstant.ADMIN_OR_TEACHER);
        }
    }
}
