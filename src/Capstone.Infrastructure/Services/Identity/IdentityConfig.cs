using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using IdentityModel;

namespace Capstone.Infrastructure.Services.Identity;

public static class IdentityConfig
{
    public const string Admin = "admin";
    public const string User = "user";
    public const string Rescue = "rescue";
    public static IEnumerable<IdentityResource> IdentityResources =>
    new List<IdentityResource>()
            {
                    new IdentityResources.OpenId(),
                    new IdentityResources.Email(),
                    new IdentityResources.Profile()
            };

    public static IEnumerable<ApiScope> ApiScopes =>
            new List<ApiScope>()
            {
                    new ApiScope("magic", "Magic Server")
            };

    public static IEnumerable<Client> Get =>
    new List<Client>()
    {
                    new Client
                    {
                        ClientId = "magic",
                        ClientSecrets = {new Secret("secret".Sha256())},
                        AllowedGrantTypes = {GrantType.ResourceOwnerPassword, OidcConstants.GrantTypes.TokenExchange},
                        AllowOfflineAccess = true,
                        AllowedScopes = { "magic",
                            IdentityServerConstants.StandardScopes.OpenId,
                            IdentityServerConstants.StandardScopes.Email,
                            IdentityServerConstants.StandardScopes.Profile,
                            IdentityServerConstants.StandardScopes.OfflineAccess
                        },
                        AccessTokenLifetime = 3600,
                        AbsoluteRefreshTokenLifetime = 2592000,
                        SlidingRefreshTokenLifetime = 1296000,
                        RefreshTokenUsage = TokenUsage.OneTimeOnly,
                        RefreshTokenExpiration = TokenExpiration.Sliding,
                        RedirectUris = { "/signin-oidc" },
                        PostLogoutRedirectUris = { "/signout-callback-oidc" },
                    }
    };
}