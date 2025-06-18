//using Capstone.Application.StudentDomain.Commands.SubmitExamSession;

//namespace Capstone.API.Endpoints.StudentEndpoint
//{
//    public class SubmitExamSession : ICarterModule
//    {
//        public void AddRoutes(IEndpointRouteBuilder app)
//        {
//            app.MapPost("/student/exam-sessions/{id}/submit", async (ISender sender, IHttpContextAccessor httpContext, Guid id) =>
//            {
//                var userId = httpContext.HttpContext!.GetUserIdFromJwt();

//                var command = new SubmitExamSessionCommand(userId, id);

//                var response = await sender.Send(command);

//                return Results.Ok(response);
//            }).RequireAuthorization(PolicyConstant.STUDENT);
//        }
//    }
//}
