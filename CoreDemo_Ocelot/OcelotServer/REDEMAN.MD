[TOC]
# 创建项目（一个server,两个webapi）

# server下载nuget `Ocelot`

# 创建`Ocelot.json` [参照地址](https://www.cnblogs.com/jesse2013/p/net-core-apigateway-ocelot-docs.html)
    ```json
        {
            "ReRoutes": [//路由配置
                //万能模板的优先级最低，只要有其它的路由模板，其它的路由模板则会优先生效。
                {
                    "DownstreamPathTemplate": "/api/{url}",//下游地址
                    "DownstreamScheme": "https",//下游服务http
                    "DownstreamHostAndPorts": [//下游地址和端口
                        {
                        "Host": "localhost",
                        "Port": 44323
                        },
                        {
                        "Host": "localhost",
                        "Port": 44308
                        }
                    ],
                    "UpstreamPathTemplate": "/api/{url}",//上游地址
                    "UpstreamHttpMethod": [ "Get","Post" ],//上游请求方式
                    "LoadBalancerOptions": {
                        "Type": "RoundRobin"//添加多个HostAndPort，时候需求指定路由负载均衡，LeastConnection – 将请求发往最空闲的那个服务器 RoundRobin – 轮流发送 NoLoadBalance – 总是发往第一个请求或者是服务发现
                    },
                    "Priority":0, //优先级 对多个产生冲突的路由设置优化级
                    "Key":"ThisKey", //唯一主键，聚合（Aggregates）使用，
                    "FileCacheOptions": {//缓存
                        "TtlSeconds": 0,
                        "Region": ""
                    },
                    "AuthenticationOptions": {//认证
                        "AuthenticationProviderKey": "TestKey",
                        "AllowedScopes": []
                    }
                    "RateLimitOptions": {//限流 对请求进行限流可以防止下游服务器因为访问过载而崩溃
                        "ClientWhitelist": [],//白名单
                        "EnableRateLimiting": true,//是否限流
                        "Period": "1s",//统计时间段：1s, 5m, 1h, 1d
                        "PeriodTimespan": 1,//多少秒之后客户端可以重试
                        "Limit":2 //在统计时间段内允许的最大请求数量
                    },
                     "QoSOptions": {//服务质量与熔断，需要安装Nuget包（Ocelot.Provider.Polly） 熔断的意思是停止将请求转发到下游服务。当下游服务已经出现故障的时候再请求也是功而返，并且增加下游服务器和API网关的负担。这个功能是用的Pollly来实现的
                        "ExceptionsAllowedBeforeBreaking": 3, //允许多少个异常请求
                        "DurationOfBreak": 10, //熔断的时间，单位为秒
                        "TimeoutValue": 5000 //如果下游请求的处理时间超过多少则自如将请求设置为超时
                    },
                    "RouteClaimsRequirement": {//鉴权 通过认证中的AllowedScopes 拿到claims之后，如果要进行权限的鉴别需要添加以下配置
                        "UserType": "registered"
                    },
                    //请求头转化 UpstreamHeaderTransform 和 DownstreamHeaderTransform
                     "UpstreamHeaderTransform": {
                        "Test": "http://www.bbc.co.uk/, http://ocelot.com/",
                    },
                    "DownstreamHeaderTransform": {
                        "Location": "{DownstreamBaseUrl}, {BaseUrl}"
                    },
                     "HttpHandlerOptions": {
                        "AllowAutoRedirect": false,
                    },
                    "AddClaimsToRequest": {//Claims转化Claims转化功能可以将Claims中的值转化到请求头、Query String、或者下游的Claims中，对于Claims的转化，比较特殊的一点是它提供了一种对字符串进行解析的方法。
                        "UserType": "Claims[sub] > value[0] > |",
                        "UserId": "Claims[sub] > value[1] > |"
                    }
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
                "Key": "sex"
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
                },
                {
                "DownstreamPathTemplate": "/api/values/{id}",
                "DownstreamScheme": "https",
                "DownstreamHostAndPorts": [
                    {
                    "Host": "localhost",
                    "Port": 44308
                    }
                ],
                "UpstreamPathTemplate": "/MapApi/values/{id}",
                "UpstreamHttpMethod": [ "Get" ],
                "ReRouteIsCaseSensitive": true
                },
                {
                "DownstreamPathTemplate": "/api/values",
                "DownstreamScheme": "https",
                "DownstreamHostAndPorts": [
                    //{
                    //  "Host": "localhost",
                    //  "Port": 44323
                    //},
                    {
                    "Host": "localhost",
                    "Port": 44308
                    }
                ],
                "UpstreamPathTemplate": "/MapApi/values",
                "UpstreamHttpMethod": [ "Get" ],

                "QoSOptions": {//服务质量与熔断 熔断的意思是停止将请求转发到下游服务。当下游服务已经出现故障的时候再请求也是功而返，并且增加下游服务器和API网关的负担。这个功能是用的Pollly来实现的

                 "ExceptionsAllowedBeforeBreaking": 3, //允许多少个异常请求

                 "DurationOfBreak": 10, //熔断的时间，单位为秒

                 "TimeoutValue": 5000 //如果下游请求的处理时间超过多少则自如将请求设置为超时

                },

                //"HttpHandlerOptions": {

                //  "AllowAutoRedirect": false,

                //  "UseCookieContainer": false

                //},

                //"AuthenticationOptions": {

                //  "AuthenticationProviderKey": "",

                //  "AllowedScopes": []

                //}
                }
            ],
            "Aggregates": [ //聚合操作，需要在ReRotes添加Key  聚合服务目前只支持返回json 目前只支持Get方式请求下游服务 任何下游的response header并会被丢弃 如果下游服务返回404，聚合服务只是这个key的value为空，它不会返回404
                {
                "ReRouteKeys": [
                    "sex",
                    "name"
                ],
                "UpstreamPathTemplate": "/user/{sex}/{name}"
                }
            ],
            "GloabalConfiguration": {
                "RateLimitOptions": {//限流
                    "DisableRateLimitHeaders": false,//Http头  X-Rate-Limit 和 Retry-After 是否禁用
                    "QuotaExceededMessage": "Customize Tips!", // 当请求过载被截断时返回的消息
                    "HttpStatusCode": 999, //当请求过载被截断时返回的http status
                    "ClientIdHeader" : "Test" //用来识别客户端的请求头，默认是 ClientId
                }
                //  "RequestIdKey": "OcrequestId",
                //  "AdministrationPath": "/administration",
                //"BaseUrl": "https://api.mybusiness.com" 网关入口
            }
            }
    ```
#  添加 consul,Nuget导入`Ocelot.Provider.Consul`,本人重新创建了个`OcelotConsul.json`
    ```json
        {
            "ReRoutes": [
                {
                "DownstreamPathTemplate": "/api/{url}",
                "DownstreamScheme": "http",
                "UpstreamPathTemplate": "/api/{url}",
                "UpstreamHttpMethod": [ "Get" ],
                "ServiceName": "servicea",//consul node
                "LoadBalancerOptions": {
                    "Type": "RoundRobin"//负责均衡
                },
                "UseServiceDiscovery": true
                }
            ],
            "GlobalConfiguration": {
                // "BaseUrl": "http://127.0.0.1:8887/"
                "ServiceDiscoveryProvider": {//全局地址
                "Host": "localhost",
                "Port": 8500,
                "Type": "PollConsul",
                "PollingInterval": 100
                }
            }
         }
    ```
# 将json文件导入项目`Program.cs`
    ```csharp
         public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
               .ConfigureAppConfiguration((hostingContext, builder) =>
            {
                builder.SetBasePath(hostingContext.HostingEnvironment.ContentRootPath)
                .AddJsonFile("Ocelot.json", false, true);
                //.AddJsonFile("OcelotConsul.json", false, true);
            })
                .UseStartup<Startup>();
    ```
# 注入服务`Startup.cs`
    ```csharp
         var authenticationProviderKey = "TestKey";
            services.AddAuthentication()
                .AddJwtBearer(authenticationProviderKey, x =>
                {
                    x.Authority = "test";
                    x.Audience = "test";
                });//认证（后面详细研究）
            services.AddOcelot() //基础调用(Ocelot)
              .AddConsul()//添加Consul服务注册(需要添加Nuget Ocelot.Provider.Consul)
              .AddPolly();//服务熔断（Ocelot.Provider.Polly）
    ```
    `app.UseOcelot().Wait();`
# IdentityServer添加
### Nuget IdentityServer4