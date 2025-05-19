using Capstone.Application.StudentDomain.Commands.JoinExamSession;

namespace Capstone.API.Endpoints.StudentEndpoint
{
    public class JoinExamSession : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/student/exam-sessions/{id}/join", async (ISender sender, IHttpContextAccessor httpContext, Guid id) =>
            {
                var userId = httpContext.HttpContext!.GetUserIdFromJwt();

                var command = new JoinExamSessionCommand(userId, id);

                var response = await sender.Send(command);

                return Results.Ok(response);
            }).RequireAuthorization(PolicyConstant.STUDENT);
        }
    }
}
