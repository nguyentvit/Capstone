using Capstone.Application.QuestionDomain.Queries.GetQuestionVersionsByQuestionId;

namespace Capstone.API.Endpoints.TeacherEndpoint
{
    public class GetQuestionVersionsByQuestionId : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/teacher/questions/{questionId}/versions", async (ISender sender, IHttpContextAccessor httpContext, Guid questionId) =>
            {
                var userId = httpContext.HttpContext!.GetUserIdFromJwt();

                var query = new GetQuestionVersionsByQuestionIdQuery(userId, questionId);

                var response = await sender.Send(query);

                return Results.Ok(response);
            }).RequireAuthorization(PolicyConstant.TEACHER);
        }
    }
}
