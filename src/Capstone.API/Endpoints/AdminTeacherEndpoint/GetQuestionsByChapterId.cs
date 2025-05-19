using Capstone.Application.QuestionDomain.Queries.GetQuestionsByChapterId;

namespace Capstone.API.Endpoints.AdminTeacherEndpoint
{
    public class GetQuestionsByChapterId : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/chapters/{id}/questions", async (ISender sender, IHttpContextAccessor httpContext, Guid id, [AsParameters] PaginationRequest paginationRequest) =>
            {
                var userId = httpContext.HttpContext!.GetUserIdFromJwt();
                var role = httpContext.HttpContext!.GetUserRoleFromJwt();

                var query = new GetQuestionsByChapterIdQuery(userId, role, id, paginationRequest);

                var response = await sender.Send(query);

                return Results.Ok(response);
            }).RequireAuthorization(PolicyConstant.ADMIN_OR_TEACHER);
        }
    }
}
