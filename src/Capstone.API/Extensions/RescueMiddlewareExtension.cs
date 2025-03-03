namespace Capstone.API.Extensions;

public static class RescueMiddlewareExtension
{
    public static IApplicationBuilder UseRescueAuthentication(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<RescueMiddleware>();
    }
}