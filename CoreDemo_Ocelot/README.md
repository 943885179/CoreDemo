# 创建三个项目（一个空项目，两个API项目）
# 启动api项目（5000和5001）
# 空项目添加Ocelot.json
  ```
   {
  "ReRoutes": [//路由模块
    {
      "DownstreamPathTemplate": "/api/values",//下游路由地址
      "DownstreamScheme": "https", //下游请求方式
      "DownstreamHostAndPorts": [//下游地址和端口（多个默认第一个）
        //{
        //  "Host": "localhost",
        //  "Port": 44323
        //},
        {
          "Host": "localhost",//地址
          "Port": 44308 //端口
        }
      ],
      "UpstreamPathTemplate": "/MapApi/values",//上游地址
      "UpstreamHttpMethod": [ "Get" ], //上游请求方式，默认Get
      "ReRouteIsCaseSensitive": true //上游地址是否区分大小写
      //"QoSOptions": {
      //  "ExceptionsAllowedBeforeBreaking": 3,
      //  "DurationOfBreak": 10,
      //  "TimeoutValue": 5000
      //},

      //"HttpHandlerOptions": {

      //  "AllowAutoRedirect": false,

      //  "UseCookieContainer": false

      //},

      //"AuthenticationOptions": {

      //  "AuthenticationProviderKey": "",

      //  "AllowedScopes": []

      //}
    },
    
   {
      "DownstreamPathTemplate": "/api/values/sex/{sex}",
      "DownstreamScheme": "https",
      "DownstreamHostAndPorts": [
        {
          "Host": "localhost",
          "Port": 44308
        }
      ],
      "UpstreamPathTemplate": "/MapApi/values/sex/{sex}",
      "UpstreamHttpMethod": [ "Get" ],
      "Key": "sex" //聚合需要指定Key,唯一
    },
    {
      "DownstreamPathTemplate": "/api/values/name/{name}",
      "DownstreamScheme": "https",
      "DownstreamHostAndPorts": [
        {
          "Host": "localhost",
          "Port": 44308
        }
      ],
      "UpstreamPathTemplate": "/MapApi/values/name/{name}",
      "UpstreamHttpMethod": [ "Get" ],
      "Key": "name"
    }
  ], 
  "Aggregates": [//聚合操作
    {
      "ReRouteKeys": [//聚合项，根据ReRouts的Key来合并
        "sex",
        "name"
      ],
      "UpstreamPathTemplate": "/user/{sex}/{name}"//聚合的上游地址
    }
  ],
  "GloabalConfiguration": {
    //  "RequestIdKey": "OcrequestId",
    //  "AdministrationPath": "/administration",
    //"BaseUrl": "https://api.mybusiness.com" 
  }
}

  ```
# 添加json到Program.cs
 ```
   public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
               .ConfigureAppConfiguration((hostingContext, builder) =>
            {
                builder.SetBasePath(hostingContext.HostingEnvironment.ContentRootPath)
                .AddJsonFile("Ocelot.json", false, true);
            })
                .UseStartup<Startup>();
 ```
# 添加Ocelot 到 Startup.cs
 ```
 
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
            //services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddOcelot();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public  void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //}
            //else
            //{
            //    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            //    app.UseHsts();
            //}
             app.UseOcelot().Wait();
            //app.UseHttpsRedirection();
             app.UseMvc();
        }
 ```
 # 关键代码：
  ```
    WebHost.CreateDefaultBuilder(args)
               .ConfigureAppConfiguration((hostingContext, builder) =>
            {
                builder.SetBasePath(hostingContext.HostingEnvironment.ContentRootPath)
                .AddJsonFile("Ocelot.json", false, true);
            })
           
            services.AddOcelot();

             
            app.UseOcelot().Wait();
  ```