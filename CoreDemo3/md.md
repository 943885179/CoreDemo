>基础知识梳理
>Program.cs 入口函数
 ```
	 public static void Main(string[] args)
        {
           CreateHostBuilder(args)
                .Build() //返回初始化完毕的IWebHebHost宿主
                .Run();//启动WebHost
           
           //或者使用这个 BuildWebHost(args).Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args) //创建CreatDefaultBuilder()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>(); // 设置启动类
                });
        public static IWebHost BuildWebHost(string[] args) => WebHost.CreateDefaultBuilder(args)
            .UseStartup<Startup>()
            .Build();
    }
 ```
>startup.cs 程序启动入口
+ 主机提供 Startup 类构造函数可用的某些服务。 应用通过 ConfigureServices 添加其他服务,依赖服务注册。 然后，主机和应用服务都可以在 Configure 和整个应用中使用，中间件注册。
   - IHostingEnvironment 按环境配置服务。
   - IConfiguration 读取配置。
   - ILoggerFactory 在记录器中创建 Startup.ConfigureServices。
   - IServiceCollection 当前容器中各服务的配置集合
   - IApplicationBuilder  应用程序管道构建