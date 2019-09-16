using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CoreDemo_Consul
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime lifetime)
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

            var consulOption = new ConsulOption()
            {
                ServiceName = Configuration["ServiceName"],
                ServiceIP = Configuration["ServiceIP"],
                ServicePort = Convert.ToInt32(Configuration["ServicePort"]),
                ServiceHealthCheck = Configuration["ServiceHealthCheck"],
                Address = Configuration["ConsulAddress"],
                
            };
            app.RegisterConsul(lifetime, consulOption);
            /*String ip = Configuration["ip"];//部署到不同服务器的时候不能写成127.0.0.1或者0.0.0.0，因为这是让服务消费者调用的地址
            int port = int.Parse(Configuration["port"]);//获取服务端口
            var client = new ConsulClient(ConfigurationOverview); //回调获取
            var result = client.Agent.ServiceRegister(new AgentServiceRegistration()
            {
                ID = "ServerNameFirst" + Guid.NewGuid(),//服务编号保证不重复
                Name = "ServerFirst",//服务的名称 集群使用consul
                Address = ip,//服务ip地址
                Port = port,//服务端口
                Check = new AgentServiceCheck //健康检查
                {
                    DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(5),//服务启动多久后反注册
                    Interval = TimeSpan.FromSeconds(10),//健康检查时间间隔，或者称为心跳间隔（定时检查服务是否健康）
                    HTTP = $"http://{ip}:{port}/api/Health",//健康检查地址
                    Timeout = TimeSpan.FromSeconds(5)//服务的注册时间
                }
            });*/
        }
        /// 获取Consul的实例
        /// </summary>
        /// <param name="obj"></param>
        private static void ConfigurationOverview(ConsulClientConfiguration obj)
        {
            //consul的地址
            obj.Address = new Uri("http://127.0.0.1:8500");
            //数据中心命名
            obj.Datacenter = "dc1";
        }
    }
}
