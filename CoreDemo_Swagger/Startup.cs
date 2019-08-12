using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;

namespace CoreDemo_Swagger
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
            services.AddSwaggerGen(options =>
            {
                //配置第一个Doc
                options.SwaggerDoc("v1", new Info
                {
                    Title = "My API_1",
                    Version = "v1",
                    Description = "这是说明信息",//说明
                    TermsOfService = "None",
                    Contact = new Contact
                    {
                        Name = "mzj",
                        Email = "943885179@qq.com",
                        Url = "https://github.com/943885179/CoreDemo"
                    },
                    License = new License
                    {
                        Name = "许可证名字",
                        Url = "https://github.com/943885179/CoreDemo"
                    }
                });
                //配置第二个Doc
                options.SwaggerDoc("v2", new Info { Title = "My API_2", Version = "v2" });
                // 为 Swagger JSON and UI设置xml文档注释路径
                var basePath = Path.GetDirectoryName(typeof(Program).Assembly.Location);//获取应用程序所在目录（绝对，不受工作目录影响，建议采用此方法获取路径）
                var xmlPath = Path.Combine(basePath, "CoreDemo_Swagger.xml");
                options.IncludeXmlComments(xmlPath);
                //var basePaths = Path.GetDirectoryName(typeof(Demo).Assembly.Location);//获取应用程序所在目录（绝对，不受工作目录影响，建议采用此方法获取路径）
                //var xmlPaths = Path.Combine(basePaths, "Cs.xml");
                //options.IncludeXmlComments(xmlPaths);
            });
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                options.SwaggerEndpoint("/swagger/v2/swagger.json", "My API V2");
                options.RoutePrefix = "swagger";
            });
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
