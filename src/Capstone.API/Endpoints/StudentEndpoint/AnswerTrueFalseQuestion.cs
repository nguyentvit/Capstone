//using Capstone.Application.StudentDomain.Commands.AnswerTrueFalseQuestion;

//namespace Capstone.API.Endpoints.StudentEndpoint
//{
//    public record AnswerTrueFalseQuestionRequest(bool Answer);
//    public class AnswerTrueFalseQuestion : ICarterModule
//    {
//        public void AddRoutes(IEndpointRouteBuilder app)
//        {
//            app.MapPost("/student/exam-sessions/{sessionId}/questions/{questionId}/true-false", async (ISender sender, IHttpContextAccessor httpContext, Guid sessionId, Guid questionId, AnswerTrueFalseQuestionRequest request) =>
//            {
//                var userId = httpContext.HttpContext!.GetUserIdFromJwt();

//                var command = new AnswerTrueFalseQuestionCommand(userId, sessionId, questionId, request.Answer);

//                var response = await sender.Send(command);

//                return Results.Ok(response);
//            }).RequireAuthorization(PolicyConstant.STUDENT);
//        }
//    }
//}
