//using Capstone.Application.StudentDomain.Commands.AddAction;
//using Capstone.Domain.ExamSessionModule.Enums;

//namespace Capstone.API.Endpoints.StudentEndpoint
//{
//    public record AddActionRequest(ActionType ActionType);
//    public class AddAction : ICarterModule
//    {
//        public void AddRoutes(IEndpointRouteBuilder app)
//        {
//            app.MapPost("/student/exam-sessions/{sessionId}/actions", async (ISender sender, IHttpContextAccessor httpContext, Guid sessionId, AddActionRequest request) =>
//            {
//                var userId = httpContext.HttpContext!.GetUserIdFromJwt();

//                var command = new AddActionCommand(userId, sessionId, request.ActionType);

//                var response = await sender.Send(command);

//                return Results.Ok(response);
//            }).RequireAuthorization(PolicyConstant.STUDENT);
//        }
//    }
//}
