[TOC]
# docker 安装consul [安装教程](https://github.com/943885179/dockerStu/blob/master/docker_consul.md)
# 创建.net core api 项目 (我只创建了一个，修复部分返回值后在iis部署)：vs2019直接启动的端口指定局域网ip和本地127.0.0.1ip有错误，必须要用locallhost,发布到iis上可以指定域名ip
# 添加appsettings.json
 ```json
  "ServiceName": "ServiceA",
  "ServiceIP": "192.168.1.60",
  "ServicePort": 8010, 
  "ServiceHealthCheck": "http://192.168.1.60:8010/api/Health",//需要添加回调控制器HealthController
  "ConsulAddress": "http://192.168.1.60:8500" //consul服务监听地址
 ```
 # 添加回调控制器HealthController
  ```csharp
    [Route("api/[controller]")]
    [ApiController]
    public class HealthController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get() => Ok("ok");
    }
  ```
# Nuget consul包
# 添加consul注册类ConsulBuilderExtensions.cs
 ```csharp
    
    public static class ConsulBuilderExtensions
    {
        public static IApplicationBuilder RegisterConsul(this IApplicationBuilder app, IApplicationLifetime lifetime, ConsulOption consulOption)
        {
            var consulClient = new ConsulClient(x =>
            {
                // consul 服务地址
                x.Address = new Uri(consulOption.Address);
            });

            var registration = new AgentServiceRegistration()
            {
                ID = Guid.NewGuid().ToString(),
                Name = consulOption.ServiceName,// 服务名
                Address = consulOption.ServiceIP, // 服务绑定IP
                Port = consulOption.ServicePort, // 服务绑定端口
                Check = new AgentServiceCheck()
                {
                    DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(5),//服务启动多久后注册
                    Interval = TimeSpan.FromSeconds(10),//健康检查时间间隔
                    HTTP = consulOption.ServiceHealthCheck,//健康检查地址
                    Timeout = TimeSpan.FromSeconds(5)
                },
                Tags=new string[] { "Test",".net core" }//标志也可以做配置，现在固定死了
            };

            // 服务注册
            consulClient.Agent.ServiceRegister(registration).Wait();

            // 应用程序终止时，服务取消注册
            lifetime.ApplicationStopping.Register(() =>
            {
                consulClient.Agent.ServiceDeregister(registration.ID).Wait();
            });
            return app;
        }
    }
 ```
# Startup.cs注入consul
 ```csharp
  var consulOption = new ConsulOption()
            {
                ServiceName = Configuration["ServiceName"],
                ServiceIP = Configuration["ServiceIP"],
                ServicePort = Convert.ToInt32(Configuration["ServicePort"]),
                ServiceHealthCheck = Configuration["ServiceHealthCheck"],
                Address = Configuration["ConsulAddress"],
                
            };
            app.RegisterConsul(lifetime, consulOption);
 ```
 # 编写client端代码（这里使用一个接口调用,这里只是大概列举了一下get和set方法）
  ```csharp
     [HttpGet("Test/{Service}/{path}")]
        public async Task<string> Test(string Service= "ServiceA", string path="values")
        {
            var url = _configuration["ConsulAddress"].ToString();

            using (var consulClient = new ConsulClient(a => a.Address = new Uri(url)))
            {
                var services = consulClient.Catalog.Service(Service).Result.Response;
                if (services != null && services.Any())
                {
                    // 模拟随机一台进行请求，这里只是测试，可以选择合适的负载均衡工具或框架
                    Random r = new Random();
                    int index = r.Next(services.Count());
                    var service = services.ElementAt(index);

                    using (HttpClient client = new HttpClient())
                    {
                        // var response = await client.GetAsync($"http://localhost:{service.ServicePort}/api/values");
                        var response = await client.GetAsync($"http://{service.ServiceAddress}:{service.ServicePort}/api/{path}");
                        var result = await response.Content.ReadAsStringAsync();
                        return result;
                    }
                }
                return "has error";
            }
        }
        [HttpPost("Tests")]
        public async Task<string> Tests([FromBody]TestDto input)
        {
            var serverUrl = _configuration["ConsulAddress"].ToString();
            using (var consulClient = new ConsulClient(a => a.Address = new Uri(serverUrl)))
            {
                var services = consulClient.Catalog.Service(input.Service).Result.Response;
                if (services != null && services.Any())
                {
                    // 模拟随机一台进行请求，这里只是测试，可以选择合适的负载均衡工具或框架
                    Random r = new Random();
                    int index = r.Next(services.Count());
                    var service = services.ElementAt(index);
                    HttpClientHandler handler = new HttpClientHandler();

                    using (HttpClient client = new HttpClient(handler))
                    {
                        //Data为json,
                        HttpContent content = new StringContent(input.Data)
                        {
                          //  Headers = { ContentType = new MediaTypeHeaderValue("application/json") }
                        };
                        // client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        // var response = await client.GetAsync($"http://{service.ServiceAddress}:{service.ServicePort}/api/{input.Path}");
                        var response = await client.PostAsync($"http://{service.ServiceAddress}:{service.ServicePort}/api/{input.Path}", content);
                        var result = await response.Content.ReadAsStringAsync();
                        return result;
                    }
                }
                return "has error";
            }
        }
  ```
# 发布IIS(只是简单的发布，第一次发布保存到一个地址，然后换个目录，修改了下返回值又重新发布一个，相当于多个项目了)
-  保证安装了AspNetCoreModule(IIS的模块查看)
-  选择添加网站，选择目录端口，然后修改应用程序池（无托管代码，经典） 其他配置不做介绍 
-  防火墙打开端口（防火墙高级设置=》进出站规则添加=》添加固定端口） 开放所有端口是无效的
# 查看服务是否注册[consul UI地址查看](http://127.0.0.1:8500/ui/dc1/services)
# [查看该服务的注册信息](http://127.0.0.1:8500/v1/catalog/service/ServiceA)  【 ServiceA 为服务名称】

# 测试调用 http://192.168.1.60:8010/api/values/c(这个具体看你自己的测试
# 学习时遇到其他问题
 - post请求参数是string或int时候不加[formbody]的时候在postman发起必须是params参数（即需要?后面添加参数例如?=xxx或者?value=xxx），一个的话不用加参数名称或者是参数名称，两个则会报错，不知道微软的设计思路，两个以上建议用实体
 - 必须发布到iis(个人问题吧，不发布iis用ip访问会报错，但是服务注册上去后就算是localhost也会解析成127.0.0.1,所以会报错)
 - 服务注册也可以使用put方法调用`http://localhost:8500/v1/catalog/register`这个接口
   + 参数为
      ```json
        {
            "Datacenter": "dc1",   
            "Node": "foobar",      
            "Address": "192.168.10.10",  
            "Service": {             //注册的服务信息
                "ID": "redis1",
                "Service": "redis",
                "Tags": [
                "primary",
                "v1"
                ],
                "Address": "127.0.0.1",
                "Port": 8000
            },
            "Check": {              //注册健康检查
                "Node": "foobar",
                "CheckID": "service:redis1",
                "Name": "Redis health check",
                "Notes": "Script based health check",
                "Status": "passing",
                "ServiceID": "redis1"
            }
        }
      ```