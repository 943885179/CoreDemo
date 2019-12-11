# Startup 类
## 主机提供 Startup 类构造函数可用的某些服务。 应用通过 ConfigureServices 添加其他服务。 主机和应用服务都可以在 Configure 和整个应用中使用。
## 使用泛型主机 (IHostBuilder) 时，只能将以下服务类型注入 Startup 构造函数：IWebHostEnvironment IHostEnvironment IConfiguration
## 应用在开发环境中运行并包含 Startup 类和 StartupDevelopment 类，则使用 StartupDevelopment 类。
## ConfigureServices 方法 :可选。在 Configure 方法配置应用服务之前，由主机调用。其中按常规设置配置选项。将服务添加到服务容器，使其在应用和 Configure 方法中可用
## Configure 方法:Configure 方法用于指定应用响应 HTTP 请求的方式。 可通过将中间件组件添加到 IApplicationBuilder 实例来配置请求管道。 Configure 方法可使用 IApplicationBuilder，但未在服务容器中注册。 托管创建 IApplicationBuilder 并将其直接传递到 Configure。
# ASP.NET Core 依赖注入 （服务）
	- AddSingleton 项目启动-项目关闭   相当于静态类  只会有一个
	- AddScoped 请求开始-请求结束  在这次请求中获取的对象都是同一个
	- AddTransient 请求获取-（GC回收-主动释放） 每一次获取的对象都不是同一个
# ASP.NET Core 中间件 Middleware
	- 使用 IApplicationBuilder 创建中间件管道
	-  Startup.Configure 方法添加中间件组件
	- 使用 Use、Run 和 Map 配置 HTTP 管道。 Use 方法可使管道短路（即不调用 next 请求委托）。 Run 是一种约定，并且某些中间件组件可公开在管道末尾运行的 Run[Middleware] 方法。
	- Map 扩展用作约定来创建管道分支。 Map 基于给定请求路径的匹配项来创建请求管道分支。 如果请求路径以给定路径开头，则执行分支。
	-内置中间件
| 描述            | 使用                       |
| --------------- | -------------------------- |
| 强制走https     | app.UseHttpsRedirection(); |
| 跨域请求 (CORS) | app.AddCors();             |
	- 自定义中间件
		- 创建类:具有类型为 RequestDelegate 的参数的公共构造函数 名为 Invoke 或 InvokeAsync 的公共方法。 此方法必须：返回 Task。接受类型 HttpContext 的第一个参数。 构造函数和 Invoke/InvokeAsync 的其他参数由依赖关系注入 (DI) 填充
		- 使用： app.UseMiddleware<FirstMiddleware>();
		- 中间件扩展方法：扩展方法通过 IApplicationBuilder 公开中间件 写静态类下静态方法，返回IApplicationBuilder ,然后就可以直接使用app.UseXxx();