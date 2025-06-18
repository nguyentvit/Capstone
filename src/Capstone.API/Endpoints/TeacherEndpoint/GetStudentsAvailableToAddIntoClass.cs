using Capstone.Application.TeacherDomain.Queries.GetStudentsAvailableToAddIntoClass;

namespace Capstone.API.Endpoints.TeacherEndpoint
{
    public class GetStudentsAvailableToAddIntoClass : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/teacher/classes/{classId}/students/available", async (ISender sender, Guid classId, [AsParameters] PaginationRequest paginationRequest, [FromQuery] string studentId = "") =>
            {
                var query = new GetStudentsAvailableToAddIntoClassQuery(classId, studentId, paginationRequest);

                var response = await sender.Send(query);

                return Results.Ok(response);
            }).RequireAuthorization(PolicyConstant.TEACHER);
        }
    }
}
