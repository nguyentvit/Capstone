//using Capstone.Application.StudentDomain.Commands.AnswerSingleChoiceQuestion;

//namespace Capstone.API.Endpoints.StudentEndpoint
//{
//    public record AnswerSingleChoiceQuestionRequest(Guid Answer);
//    public class AnswerSingleChoiceQuestion : ICarterModule
//    {
//        public void AddRoutes(IEndpointRouteBuilder app)
//        {
//            app.MapPost("/student/exam-sessions/{sessionId}/questions/{questionId}/single-choice", async (ISender sender, IHttpContextAccessor httpContext, Guid sessionId, Guid questionId, AnswerSingleChoiceQuestionRequest request) =>
//            {
//                var userId = httpContext.HttpContext!.GetUserIdFromJwt();

//                var command = new AnswerSingleChoiceQuestionCommand(userId, sessionId, questionId, request.Answer);

//                var response = await sender.Send(command);

//                return Results.Ok(response);
//            }).RequireAuthorization(PolicyConstant.STUDENT);
//        }
//    }
//}
