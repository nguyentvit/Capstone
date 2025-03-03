
namespace Capstone.API.Endpoints.UserAccess;
public class GetRescuesManage : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/users/rescues/manage", async (IHttpContextAccessor httpContext, ISender sender) => {
            var userId = httpContext.HttpContext!.GetUserIdFromJwt();
        }).RequireAuthorization();
    }
}
