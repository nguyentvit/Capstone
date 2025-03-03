namespace Capstone.API.Endpoints.UserAccess;

public class GetInfo : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/users/info", async (ISender sender, IHttpContextAccessor httpContext) => {
            var userId = httpContext.HttpContext!.GetUserIdFromJwt();
        }).RequireAuthorization();
    }
}
