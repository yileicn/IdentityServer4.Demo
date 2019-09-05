using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IDServer
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResource("CustomerPofile",new List<string>{"role"})
            };
        }

        public static IEnumerable<ApiResource> GetApis()
        {
            return new List<ApiResource>
            {
                new ApiResource("api1.read", "server api1 read", new List<string>{"role" }),
                new ApiResource("api1.write", "server api1 write", new List<string>{"role" }),
                new ApiResource("api2", "mvc client api")
                {
                    Scopes =
                    {
                        new Scope()
                        {
                            Name = "api/values",
                            DisplayName = "Full access to API 2"
                        },
                        new Scope
                        {
                            Name = "api2.read_only",
                            DisplayName = "Read only access to API 2"
                        }
                    }
                },
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                //use the clientid/secret for authentication client(Console)
                new Client
                {
                    ClientId = "client",

                    // no interactive user, use the clientid/secret for authentication
                    AllowedGrantTypes = GrantTypes.ClientCredentials,

                    // secret for authentication
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },

                    // scopes that client has access to
                    AllowedScopes = { "api1.read" }
                },
                //OpenID Connect implicit flow client (MVC)
                new Client
                {
                    ClientId = "mvc",
                    ClientName = "MVC Client",
                    //AllowedGrantTypes = GrantTypes.Implicit,
                    AllowedGrantTypes = GrantTypes.Hybrid,

                    // secret for authentication
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },

                    // where to redirect to after login
                    RedirectUris = { "http://localhost:5002/signin-oidc" },

                    // where to redirect to after logout
                    PostLogoutRedirectUris = { "http://localhost:5002/signout-callback-oidc" },

                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "api1.read",
                        "api1.write"
                    },
                    //这允许请求刷新令牌以实现长期存在的API访问
                    AllowOfflineAccess = true,
                    //总是包含用户Claim到IdToken
                    AlwaysIncludeUserClaimsInIdToken = true
                }
            };
        }

        public static List<TestUser> GetUsers()
        {
            return new List<TestUser>
            {
                new TestUser
                {
                    SubjectId = "1",
                    Username = "admin",
                    Password = "admin",

                    Claims = new []
                    {
                        new Claim("name", "admin"),
                        new Claim("role","admin"),
                        new Claim("website", "https://admin.com")
                    }
                },
                new TestUser
                {
                    SubjectId = "2",
                    Username = "yilei",
                    Password = "yilei",

                    Claims = new []
                    {
                        new Claim("name", "yilei"),
                        new Claim("role","user"),
                        new Claim("website", "https://yilei.com")
                    }
                }
            };
        }
    }
}
