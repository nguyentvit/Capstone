using System.Text.RegularExpressions;
using Capstone.Application.Interface;
using Capstone.Domain.RescueTeam.ValueObjects;

namespace Capstone.API.Middlewares;

public class RescueMiddleware(RequestDelegate next)
{
    private static readonly Regex RescueUrlPattern = new(@"^/rescues/([0-9a-fA-F-]+)(/.*)?$", RegexOptions.Compiled);
    public async Task InvokeAsync(HttpContext context, IApplicationDbContext dbContext)
    {
        var path = context.Request.Path.ToString().ToLower();
        var match = RescueUrlPattern.Match(path);

        if (!match.Success)
        {
            await next(context);
            return;
        }

        if (!Guid.TryParse(match.Groups[1].Value, out var guidSecureId))
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsync("Invalid secureId format.");
            return;
        }

        var rescueId = RescueId.Of(guidSecureId);
        var userId = context.GetUserIdFromJwt();

        var rescue = await dbContext.Rescues.FindAsync([rescueId]);

        if (rescue == null || rescue.ManagerId.Value != userId)
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            await context.Response.WriteAsync("Access denied.");
            return;
        }

        await next(context);
    }
}
