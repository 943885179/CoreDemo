using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace servers
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = "Cookies";
                options.DefaultChallengeScheme = "oidc";
            })
            .AddCookie("Cookies")
            /*.AddQQ("QQ", qqOptions =>
             {
                 qqOptions.AppId = "<insert here>";
                 qqOptions.AppKey = "<insert here>";
             })*/
            //openId授权
           .AddOpenIdConnect("oidc", options =>
           {
               options.SignInScheme = "Cookies";

               options.Authority = "https://localhost:5001";
               options.RequireHttpsMetadata = false;

               options.ClientId = "ImpLicitClient";
               options.SaveTokens = true;
           });/* 
            .AddOpenIdConnect("oidc", options =>
            {
                options.SignInScheme = "Cookies";

                options.Authority = "https://localhost:5001";
                options.RequireHttpsMetadata = true;

                options.ClientId = "mvc";
                options.ClientSecret = "secret";
                options.ResponseType = "code id_token";
                options.SignedOutRedirectUri = "https://localhost:5008";
                options.SaveTokens = true;
                 //布尔值来设置处理程序是否应该转到用户信息端点检索。额外索赔或不在id_token创建一个身份收到令牌端点。默认为“false”
                options.GetClaimsFromUserInfoEndpoint = true;
                options.Scope.Add("api1");
                options.Scope.Add("api2");
                options.Scope.Add("offline_access");
                options.ClaimActions.MapJsonKey("website", "website");
            });*/
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
