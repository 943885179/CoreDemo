>基础知识梳理

>startup类
+ 主机提供 Startup 类构造函数可用的某些服务。 应用通过 ConfigureServices 添加其他服务。 然后，主机和应用服务都可以在 Configure 和整个应用中使用。
  - IHostingEnvironment 按环境配置服务。
   - IConfiguration 读取配置。
   - ILoggerFactory 在记录器中创建 Startup.ConfigureServices。