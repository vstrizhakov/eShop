using Duende.IdentityServer;
using Duende.IdentityServer.Models;

namespace eShop.Identity;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
        };

    public static IEnumerable<ApiScope> ApiScopes =>
        new ApiScope[]
        {
        };

    public static IEnumerable<Client> Clients =>
        new Client[]
        {
            new Client
            {
                ClientId = "main",
                ClientSecrets =
                {
                    new Secret("8E643505-E1B5-4F8A-8120-E84D50E1CAF6".Sha256())
                },

                AllowedGrantTypes = GrantTypes.Code,

                RedirectUris = { "https://localhost:7121/signin-oidc" },
                FrontChannelLogoutUri = "https://localhost:7121/signout-oidc",
                PostLogoutRedirectUris = { "https://localhost:7121/signout-callback-oidc" },

                AllowOfflineAccess = true,
                AllowedScopes =
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                },
            },
        };
}
