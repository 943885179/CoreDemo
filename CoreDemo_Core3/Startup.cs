using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreDemo_Core3.Middleware;
using CoreDemo_Core3.Test;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CoreDemo_Core3
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
            services.AddControllers(); 
           /* services.AddSingleton(typeof(ILogger<MyDependency>), typeof(Logger<MyDependency>));
            services.AddScoped<IMyDependency, MyDependency>();
            services.AddSingleton<IMyDependency, MyDependency>();
            services.AddTransient<IMyDependency, MyDependency>();*/
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // app.UseMiddleware<FirstMiddleware>();
            app.UseFisrt();
            app.Map("/map1", HandMapTest1);
            app.Map("/map2", HandMapTest2);
            app.MapWhen(context => context.Request.Query.ContainsKey("branch"),
                              HandleBranch);

            app.Run(async context => await context.Response.WriteAsync("nomap"));
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
        private static void HandMapTest1(IApplicationBuilder app)
        {
            app.Run(async context => await context.Response.WriteAsync("map1"));
        }
        private static void HandMapTest2(IApplicationBuilder app)
        {
            app.Run(async context => await context.Response.WriteAsync("map2"));
        }
        private static void HandleBranch(IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                var branchVer = context.Request.Query["branch"];
                await context.Response.WriteAsync($"Branch used = {branchVer}");
            });
        }
    }
}
