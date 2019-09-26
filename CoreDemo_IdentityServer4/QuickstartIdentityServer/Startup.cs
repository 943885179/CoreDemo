using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace QuickstartIdentityServer
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            // 使用内存存储，密钥，客户端和资源来配置身份服务器。
            services.AddIdentityServer()//赖注入系统中注册IdentityServer,注入DI
                .AddDeveloperSigningCredential()//在每次启动时，为令牌签名创建了一个临时密钥。在生成环境需要一个持久化的密钥
                .AddInMemoryIdentityResources(Config.GetIdentityResources())
                .AddInMemoryApiResources(Config.GetApis())
                .AddInMemoryClients(Config.GetClients())
                .AddTestUsers(Config.GetTestUsers());
            /*services.AddAuthentication()
             //添加Google第三方身份认证服务（按需添加）
             .AddGoogle("Google", options =>
             {
                 options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
                 options.ClientId = "434483408261-55tc8n0cs4ff1fe21ea8df2o443v2iuc.apps.googleusercontent.com";
                 options.ClientSecret = "3gcoTrEDPPJ0ukn_aYYT6PWo";
             })
             //如果当前IdentityServer不提供身份认证服务，还可以添加其他身份认证服                务提供商
             .AddOpenIdConnect("oidc", "OpenID Connect", options =>
             {
                 options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
                 options.SignOutScheme = IdentityServerConstants.SignoutScheme;
                 options.Authority = "https://demo.identityserver.io/";
                 options.ClientId = "implicit";
                 options.TokenValidationParameters = new TokenValidationParameters
                 {
                     NameClaimType = "name",
                     RoleClaimType = "role"
                 };
             });*/
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseIdentityServer();
            app.UseStaticFiles();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
