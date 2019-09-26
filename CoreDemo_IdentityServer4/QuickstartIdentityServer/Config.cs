using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace QuickstartIdentityServer
{
    public static class Config
    {
        //定义 API
        //范围（Scopes）用来定义系统中你想要保护的资源，比如 API。
        public static IEnumerable<ApiResource> GetApis()
        {
            return new List<ApiResource>
            {
                new ApiResource("api1", "My API")
            };
        }
        //定义 客户端
        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = "client",
                    // 没有交互性用户，使用 clientid/secret 实现认证。
                    // no interactive user, use the clientid/secret for authentication
                    AllowedGrantTypes = GrantTypes.ClientCredentials,//客户模式,

                    // secret for authentication
                    //用于认证的密码
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },

                    // scopes that client has access to
                    //客户端有权访问的范围（Scopes）
                    AllowedScopes = { "api1" }
                },
                new Client()
                {
                    ClientId="PasswordClient",
                    AllowedGrantTypes=GrantTypes.ResourceOwnerPassword,//密码模式
                    ClientSecrets={ new Secret("secretPasswordxxxxx".Sha256())},
                    AllowedScopes={ "api1"}
                },
                new Client()
                {
                    ClientId="ImpLicitClient",
                    AllowedGrantTypes=GrantTypes.Implicit,//OpenID Connect 简化模式（implicit）
                    ClientSecrets={ new Secret("implicitSecrets".Sha256()) },
                    RequireConsent=true,   //用户选择同意认证授权
                    RedirectUris={ "https://localhost:5008/signin-oidc" }, //指定允许的URI返回令牌或授权码(我们的客户端地址)
                    PostLogoutRedirectUris={ "https://localhost:5008/signout-callback-oidc" },//注销后重定向地址 
                    LogoUri="https://ss1.bdstatic.com/70cFuXSh_Q1YnxGkpoWK1HF6hhy/it/u=3298365745,618961144&fm=27&gp=0.jpg",
                  ////运行访问的资源
                  AllowedScopes = {                       //客户端允许访问个人信息资源的范围
                      IdentityServerConstants.StandardScopes.Profile,
                      IdentityServerConstants.StandardScopes.OpenId,
                      IdentityServerConstants.StandardScopes.Email,
                      IdentityServerConstants.StandardScopes.Address,
                      IdentityServerConstants.StandardScopes.Phone,
                      "api1"
                  }

                }
            };
        }
        public static List<TestUser> GetTestUsers()
        {
            return new List<TestUser>() {
               /*  new TestUser()
                {
                    SubjectId="1",
                    Username="weixiao",
                    Password="123"
                },*/
                new TestUser
                {
                    SubjectId = "2",
                    Username = "admin",
                    Password = "123456",

                    Claims = new List<Claim>
                    {
                        new Claim("name", "adminssss"),
                        new Claim("website", "https://www.cnblogs.com/linezero")
                    }
                },
            };
        }
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email(),
                new IdentityResources.Phone()
            };
        }
    }
}
