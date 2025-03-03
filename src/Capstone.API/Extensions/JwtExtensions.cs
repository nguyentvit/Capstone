using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Capstone.API.Extensions;
public static class JwtExtensions
{
    public static Guid GetUserIdFromJwt(this HttpContext httpContext)
    {
        var token = httpContext.GetJwtToken();
        if (string.IsNullOrEmpty(token)) return Guid.Empty;

        var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
        var userIdClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "id")?.Value;
        if (Guid.TryParse(userIdClaim, out Guid userId))
        {
            return userId;
        }
        return Guid.Empty;
    }
    public static string GetUserRoleFromJwt(this HttpContext httpContext)
    {
        var token = httpContext.GetJwtToken();
        if (string.IsNullOrEmpty(token)) return null!;

        var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
        return jwtToken.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Role)?.Value!;
    }
    private static string GetJwtToken(this HttpContext httpContext)
    {
        var authorizationHeader = httpContext.Request.Headers["Authorization"].FirstOrDefault();
        if (authorizationHeader != null && authorizationHeader.StartsWith("Bearer "))
        {
            return authorizationHeader.Substring("Bearer ".Length).Trim();
        }

        return null!;
    }
}