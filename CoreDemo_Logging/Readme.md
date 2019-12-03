#.net Core 操作日志
## 简单使用
	```csharp
		public class ValuesController : ControllerBase
			{
				ILogger<ValuesController> logger;
				//构造函数注入Logger
				public ValuesController(ILogger<ValuesController> logger)
				{
					this.logger = logger;
				}
				[HttpGet]
				public IEnumerable<string> Get()
				{
					logger.LogWarning("Warning");
					logger.LogError("Error");
					logger.LogDebug("Debug");
					return new string[] { "value1", "value2" };
				}
			}
	```
## 添加Log4Net
### Nuget安装`Microsoft.Extensions.Logging.Log4Net.AspNetCore`
### 注册方式一
	```csharp
		public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
            .ConfigureLogging((context, logger) =>{
                logger.AddFilter("System", LogLevel.Warning);
                logger.AddFilter("Microsoft", LogLevel.Warning);
                logger.AddLog4Net();
            })
            .ConfigureLogging(option=> {
                option.ClearProviders();
                option.AddConsole();
            });
	```
### 注册方式二:Startup.cs中添加ILoggerFactory 注入AddLog4Net();
	```csharp
		 public void Configure(IApplicationBuilder app, IHostingEnvironment env,ILoggerFactory logger)
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
            logger.AddLog4Net();
            app.UseHttpsRedirection();
            app.UseMvc();
        }
	```
	>使用：和简单使用一致或者用以下的方法调用（此时调用会报错`FileNotFoundException: Could not find file 'F:\CoreStu\CoreDemo\CoreDemo_Logging\bin\Debug\netcoreapp2.2\log4net.config'`需要先配置log4net.config)
	```csharp
		var logger=new LoggerFactory().AddLog4Net().CreateLogger("logs");
		logger.LogError("这是错误信息");
	```
### 添加Log4Net.Config文件
	```xml
		<log4net>
		  <root>
			<level value="ALL" ref="infoAppender" />
			<appender-ref ref="infoAppender" />
			<appender-ref ref="errorAppender" />
			<appender-ref ref="debugAppender" />
		  </root>

		  <!-- 日志的等级，它们由高到底分别为： OFF > FATAL > ERROR > WARN > INFO > DEBUG > ALL -->
		  <!--信息日志配置-->
		  <appender name="infoAppender" type="log4net.Appender.RollingFileAppender">
			<param name="File" value="Logs\Info\info.log" />
			<param name="AppendToFile" value="true" />
			<param name="MaxFileSize" value="10240" />
			<param name="MaxSizeRollBackups" value="100" />
			<param name="PreserveLogFileNameExtension" value="true" />
			<param name="StaticLogFileName" value="false" />
			<param name="DatePattern" value="yyyyMMdd" />
			<param name="RollingStyle" value="Date" />
			<layout type="log4net.Layout.PatternLayout">
			  <conversionPattern value="%date [%thread] %-5level %logger - %message%newline" />
			</layout>
			<filter type="log4net.Filter.LevelRangeFilter">
			  <param name="LevelMin" value="INFO" />
			  <param name="LevelMax" value="INFO" />
			</filter>
		  </appender>

		  <!--调试日志配置-->
		  <appender name="debugAppender" type="log4net.Appender.RollingFileAppender">
			<param name="File" value="Logs\Debug\debug.log" />
			<param name="AppendToFile" value="true" />
			<param name="MaxFileSize" value="10240" />
			<param name="MaxSizeRollBackups" value="100" />
			<param name="PreserveLogFileNameExtension" value="true" />
			<param name="StaticLogFileName" value="false" />
			<param name="DatePattern" value="yyyyMMdd" />
			<param name="RollingStyle" value="Date" />
			<layout type="log4net.Layout.PatternLayout">
			  <conversionPattern value="%date [%thread] %-5level %logger - %message%newline" />
			</layout>
			<filter type="log4net.Filter.LevelRangeFilter">
			  <param name="LevelMin" value="DEBUG" />
			  <param name="LevelMax" value="DEBUG" />
			</filter>
		  </appender>

		  <!--错误日志配置-->
		  <appender name="errorAppender" type="log4net.Appender.RollingFileAppender">
			<param name="File" value="Logs\Error\Err.log" />
			<param name="AppendToFile" value="true" />
			<param name="MaxFileSize" value="10240" />
			<param name="MaxSizeRollBackups" value="100" />
			<param name="PreserveLogFileNameExtension" value="true" />
			<param name="StaticLogFileName" value="false" />
			<param name="DatePattern" value="yyyyMMdd" />
			<param name="RollingStyle" value="Date" />
			<layout type="log4net.Layout.PatternLayout">
			  <conversionPattern value="%date [%thread] %-5level %logger - %message%newline" />
			</layout>
			<filter type="log4net.Filter.LevelRangeFilter">
			  <param name="LevelMin" value="ERROR" />
			  <param name="LevelMax" value="ERROR" />
			</filter>
		  </appender>
		</log4net>
	```
#### 参考 [Log4Net使用](https://github.com/WeihanLi/WeihanLi.Common/tree/dev/src/WeihanLi.Common.Logging.Log4Net)