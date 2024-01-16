using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using System.Security.Claims;

namespace eShopping.Identity;

public static class Config
{
    public const string IdentityAccountScope = "account";
    public const string ApiScope = "api";

    public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            new IdentityResources.Phone(),
            new IdentityResource
            {
                Name = IdentityAccountScope,
                UserClaims =
                {
                    "account_id",
                },
            }
        };

    public static IEnumerable<ApiScope> ApiScopes =>
        new ApiScope[]
        {
            new ApiScope(ApiScope, "Main API"),
        };

    public static IEnumerable<ApiResource> ApiResources =>
        new ApiResource[]
        {
            new ApiResource
            {
                Name = ApiScope,
                Scopes =
                {
                    ApiScope,
                },
                UserClaims =
                {
                    "account_id",
                },
            },
        };

    public static IEnumerable<Client> Clients(string host) =>
        new Client[]
        {
            new Client
            {
                ClientId = "client",
                AllowedGrantTypes = GrantTypes.Code,
                RequireClientSecret = false,
                ClientSecrets =
                {
                    new Secret("8B078554F0C14B66AB94C58467B3E25B".Sha256()),
                },
                AllowedScopes =
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    IdentityServerConstants.StandardScopes.Phone,
                    IdentityAccountScope,
                    ApiScope,
                },
                RedirectUris =
                {
                    $"{host}/auth/signIn/callback",
                },
                PostLogoutRedirectUris =
                {
                    $"{host}/auth/signOut/callback",
                },
                AllowOfflineAccess = true,
                AllowedCorsOrigins =
                {
                    host,
                },
                AlwaysIncludeUserClaimsInIdToken = true,
            },
        };
}
