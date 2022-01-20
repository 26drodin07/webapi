using IdentityServer4.Models;
using System.Collections.Generic;

namespace IdentityServer.Configuration
{
    public static class IdentityConfiguration
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
           new List<IdentityResource>
           {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
           };

        public static IEnumerable<ApiScope> ApiScopes =>
            new List<ApiScope>
            {
                new ApiScope("InnovationRegistryApi", "InnovationRegistry API")
            };

        public static IEnumerable<Client> Clients =>
            new List<Client>
            {
                new Client
                {
                    ClientId = "InnovationRegistryNg",
                    ClientName = "Angular client of InnovationRegistry app",

                    AllowedGrantTypes = GrantTypes.Code,

                    //secrets for SPA clients are useless
                    RequireClientSecret = false,

                    //for refresh-token feature
                    AllowOfflineAccess = true,
                    //onetime tokens for SPA client
                    RefreshTokenUsage = TokenUsage.OneTimeOnly,

                    // scopes that client has access to
                    AllowedScopes = { "openid", "profile", "InnovationRegistryApi" },
                    AllowedCorsOrigins = { "http://localhost:4200" },
                    RedirectUris =
                    {
                        "http://localhost:4200/index.html"
                    }
                }
            };
    }
}
