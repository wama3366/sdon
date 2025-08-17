using System.Collections.Generic;
using Duende.IdentityModel;
using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Test;

namespace IdentityAndAccess.DbMigrations;

public static class Config
{
    public static List<TestUser> TestUsers =>
    [
        new()
        {
            SubjectId = "1",
            Username = "wassim",
            Password = "wassim"
        }
    ];

    public static IEnumerable<Client> TestClients =>
    [
        // TODO: define mobile apps, websites, 3rd party clients here. Each with the appropriate scopes and grant type.

        // Machine to machine client
        // Server provides ClientId and hashed ClientSecret and they receieve an access_token with the allowed scopes.
        // Use to allow non-user specific actions, like imports, exports, scheduled jobs, etc...
        new()
        {
            AllowedGrantTypes = GrantTypes.ClientCredentials,
            ClientId = "M2M",
            ClientSecrets = { new Secret("secret".Sha256()) },
            AllowedScopes = { "SchoolDonations.Api.Read", "SchoolDonations.Api.Write" }
        },
        // Oidc client
        // User is redirected to log in to IdentityServer and gets an authorization code sent to the redirect URL. (Authorize endpoint)
        // The app then exchanges the code for an access_token and optionally a refresh token and ID token. (Token endpoint)
        // Use for apps that interact with users.
        // Safe to use on public clients that support PKCE
        new()
        {
            // Authorization Code flow.
            AllowedGrantTypes = GrantTypes.Code,
            ClientId = "OidcClient",
            ClientSecrets = { new Secret("secret".Sha256()) },
            // Static Urls. Don't need to be real. They're automatically implemented by IdentityServer on the client side.
            RedirectUris = { "https://localhost:7001/signin-oidc" },
            PostLogoutRedirectUris = { "https://localhost:7001/signout-callback-oidc" },

            // To allow requesting refresh tokens.
            AllowOfflineAccess = true,

            // Scopes. Use those to restrict the client's access to resources.
            AllowedScopes =
            {
                IdentityServerConstants.StandardScopes.OpenId,
                IdentityServerConstants.StandardScopes.Profile,
                "SchoolDonations.Api.Read",
                "SchoolDonations.Api.Write",
            }
        }
    ];

    // Identity resources represent identity data about a user that can be requested via the scope parameter (OpenID Connect)
    public static readonly IEnumerable<IdentityResource> IdentityResources =
    [
        // some standard scopes from the OIDC spec
        new IdentityResources.OpenId(),
        new IdentityResources.Profile(),
        new IdentityResources.Email()
    ];

    // API scopes represent values that describe scope of access and can be requested by the scope parameter (OAuth)
    public static readonly IEnumerable<ApiScope> ApiScopes =
    [
        // local API scope
        new ApiScope(IdentityServerConstants.LocalApi.ScopeName),

        // resource specific scopes
        new ("resource1.scope1"),
        new ("resource1.scope2"),

        new ("resource2.scope1"),
        new ("resource2.scope2"),

        new ("resource3.scope1"),
        new ("resource3.scope2"),

        // a scope without resource association
        new ("scope3"),
        new ("scope4"),

        // a scope shared by multiple resources
        new ("shared.scope"),

        // a parameterized scope
        new ApiScope("transaction", "Transaction")
        {
            Description = "Some Transaction"
        },

        new("SchoolDonations.Api.Read", "School Donations"),
        new("SchoolDonations.Api.Write", "School Donations")
    ];

    // API resources are more formal representation of a resource with processing rules and their scopes (if any)
    public static readonly IEnumerable<ApiResource> ApiResources =
    [
        new ("urn:resource1", "Resource 1")
        {
            ApiSecrets = { new Secret("secret".Sha256()) },

            Scopes = { "resource1.scope1", "resource1.scope2", "shared.scope" }
        },

        new ("urn:resource2", "Resource 2")
        {
            ApiSecrets = { new Secret("secret".Sha256()) },

            // additional claims to put into access token
            UserClaims =
            {
                JwtClaimTypes.Name,
                JwtClaimTypes.Email
            },

            Scopes = { "resource2.scope1", "resource2.scope2", "shared.scope" }
        },

        new ("urn:resource3", "Resource 3 (isolated)")
        {
            ApiSecrets = { new Secret("secret".Sha256()) },

            RequireResourceIndicator = true,
            Scopes = { "resource3.scope1", "resource3.scope2", "shared.scope" }
        }
    ];
}