using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Reflection;
namespace QuickstartIdentityServer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            var op = Configuration.GetConnectionString("Default");
            services.AddDbContext<MyDbContext>(options => options.UseSqlServer(op));
            //services.AddIdentity<User, IdentityRole>() .AddEntityFrameworkStores<MyDbContext>().AddDefaultTokenProviders();
            // services.AddDbContext<MyDbContext>(options => options.UseMySql(op));
            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;
            // 使用内存存储，密钥，客户端和资源来配置身份服务器。
            /* services.AddIdentityServer()
                    .AddDeveloperSigningCredential()
                    .AddInMemoryPersistedGrants()
                    .AddInMemoryIdentityResources(Config.GetIdentityResources())
                    .AddInMemoryApiResources(Config.GetApis())
                    .AddInMemoryClients(Config.GetClients())*/
            services.AddIdentityServer()//赖注入系统中注册IdentityServer,注入DI
                .AddDeveloperSigningCredential()//在每次启动时，为令牌签名创建了一个临时密钥。在生成环境需要一个持久化的密钥
                                                /*.AddInMemoryIdentityResources(Config.GetIdentityResources())
                                                .AddInMemoryApiResources(Config.GetApis())
                                                .AddInMemoryClients(Config.GetClients())*/

                //.AddTestUsers(Config.GetTestUsers())
                .AddConfigurationStore(options =>
                {
                    options.ConfigureDbContext = b => b.UseSqlServer(op,
                              sql => sql.MigrationsAssembly(migrationsAssembly));
                })
                .AddOperationalStore(options =>
                {
                    options.ConfigureDbContext = b => b.UseSqlServer(op, sql => sql.MigrationsAssembly(migrationsAssembly));
                    options.EnableTokenCleanup = true;
                    options.TokenCleanupInterval = 30;
                });
                //.AddAspNetIdentity<User>();

            /* mysql配置（mysql版本太高会保存,需要改my.ini sql-mode="NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION"）
             .AddConfigurationStore(options => {
                 options.ConfigureDbContext = b => b.UseMySql(op, sql => sql.MigrationsAssembly(migrationsAssembly));
             })
             .AddOperationalStore(options => {
                 options.ConfigureDbContext = b => b.UseMySql(op, sql => sql.MigrationsAssembly(migrationsAssembly));
                 options.EnableTokenCleanup = true;
                 options.TokenCleanupInterval = 30;
             });*/
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
            InitializeDatabase(app);
            app.UseIdentityServer();
            app.UseStaticFiles();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
        private void InitializeDatabase(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                serviceScope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();

                var context = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
                context.Database.Migrate();
                if (!context.Clients.Any())
                {
                    foreach (var client in Config.GetClients())
                    {
                        context.Clients.Add(client.ToEntity());
                    }
                    context.SaveChanges();
                }

                if (!context.IdentityResources.Any())
                {
                    foreach (var resource in Config.GetIdentityResources())
                    {
                        context.IdentityResources.Add(resource.ToEntity());
                    }
                    context.SaveChanges();
                }

                if (!context.ApiResources.Any())
                {
                    foreach (var resource in Config.GetApis())
                    {
                        context.ApiResources.Add(resource.ToEntity());
                    }
                    context.SaveChanges();
                }
            }
        }
    }
}
