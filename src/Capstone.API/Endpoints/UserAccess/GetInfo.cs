using Capstone.Application.UserAccess.Queries.GetUserInfo;

namespace Capstone.API.Endpoints.UserAccess;
public class GetInfo : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/users/info", async (ISender sender, IHttpContextAccessor httpContext) => {
            var userId = httpContext.HttpContext!.GetUserIdFromJwt();

            var query = new GetUserInfoQuery(userId);

            var response = await sender.Send(query);

            return Results.Ok(response);
        }).RequireAuthorization();
    }
}
