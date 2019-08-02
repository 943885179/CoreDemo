using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using CoreDemo_AutoFac.Test;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CoreDemo_AutoFac
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMemoryCache();//使用内存缓存
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            return RegisterAutofac(services);//注册Autofac
        }

        private IServiceProvider RegisterAutofac(IServiceCollection services)
        {
            //实例化Autofac容器
            var builder = new ContainerBuilder();
            //将Services中的服务填充到Autofac中
            builder.Populate(services);
            //新模块组件注册    
            builder.RegisterModule<AutofacModuleRegister>();
            //创建容器
            var Container = builder.Build();
            //第三方IOC接管 core内置DI容器 
            return new AutofacServiceProvider(Container);
        }
        public class AutofacModuleRegister : Autofac.Module
        {
            //重写Autofac管道Load方法，在这里注册注入
            protected override void Load(ContainerBuilder builder)
            {
                //注册Samoyed指定为IDog实现
                //builder.RegisterType<Cat>().As<IAnimal>();
                //builder.RegisterType<Dog>().As<IAnimal>();
                var ss = typeof(Startup).Assembly;
                var ts = GetAssemblyByName("BLLS");
                builder.RegisterAssemblyTypes(typeof(Startup).Assembly).AsImplementedInterfaces(); //这里的AsImplementedInterfaces表示以接口的形式注册

                //单独注册
                builder.RegisterType<Cat>().Named<IAnimal>(typeof(Cat).Name);
                builder.RegisterType<Dog>().Named<IAnimal>(typeof(Dog).Name);

                builder.RegisterAssemblyTypes(GetAssemblyByName("BLLS")).Where(a => a.Name.EndsWith("Service")).AsImplementedInterfaces();
            }
            /// <summary>
            /// 根据程序集名称获取程序集
            /// </summary>
            /// <param name="AssemblyName">程序集名称</param>
            /// <returns></returns>
            public static Assembly GetAssemblyByName(String AssemblyName)
            {
                return Assembly.Load(AssemblyName);
            }
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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
