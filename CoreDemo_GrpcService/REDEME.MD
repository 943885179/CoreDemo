gRPC 是一个由Google开源的，跨语言的，高性能的远程过程调用（RPC）框架。 gRPC使客户端和服务端应用程序可以透明地进行通信，并简化了连接系统的构建。它使用HTTP/2作为通信协议，使用 Protocol Buffers 作为序列化协议。

它的主要优点：

现代高性能轻量级 RPC 框架。
约定优先的 API 开发，默认使用 Protocol Buffers 作为描述语言，允许与语言无关的实现。
可用于多种语言的工具，以生成强类型的服务器和客户端。
支持客户端，服务器双向流调用。
通过Protocol Buffers二进制序列化减少网络使用。
使用 HTTP/2 进行传输
这些优点使gRPC非常适合：

高性能轻量级微服务 - gRPC设计为低延迟和高吞吐量通信，非常适合需要高性能的轻量级微服务。
多语言混合开发 - gRPC工具支持所有流行的开发语言，使gRPC成为多语言开发环境的理想选择。
点对点实时通信 - gRPC对双向流调用提供出色的支持。gRPC服务可以实时推送消息而无需轮询。
网络受限环境 - 使用 Protocol Buffers二进制序列化消息，该序列化始终小于等效的JSON消息，对网络带宽需求比JSON小。
不建议使用gRPC的场景：

浏览器可访问的API - 浏览器不完全支持gRPC。虽然gRPC-Web可以提供浏览器支持，但是它有局限性，引入了服务器代理
广播实时通信 - gRPC支持通过流进行实时通信，但不存在向已注册连接广播消息的概念
进程间通信 - 进程必须承载HTTP/2才能接受传入的gRPC调用，对于Windows，进程间通信管道是一种更快速的方法。

# gRPC on .NET Core
Grpc.AspNetCore 一个用于在ASP.NET Core承载gRPC服务的框架，将 gRPC和ASP.NET Core 功能集成在一起，如：日志、依赖注入、身份认证和授权。
Grpc.Net.Client 基于HttpClient （HttpClient现已支持HTTP/2）的 gRPC客户端
Grpc.Net.ClientFactory 与gRPC客户端集成的HttpClientFactory，允许对gRPC客户端进行集中配置，并使用DI注入到应用程序中
# 源码解析
## 服务端
配置文件 appsettings.json ，多了Kestrel 启用 HTTP/2 的配置，因为 gRPC 是基于 HTTP/2 来通信的
```csharp
	app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<GreeterService>();

                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
                });
            });
```
greet.proto：greet.proto 文件定义 Greeter gRPC，且用于生成 gRPC 服务器资产。
Services 文件夹：包含 Greeter 服务的实现。
appSettings.json：包含配置数据，如 Kestrel 使用的协议。
Program.cs:包含 gRPC 服务的入口点。
Startup.cs:IWebHostBuilder的启动配置文件,包含配置应用行为的代码。
## 客户端
Program.cs 包含 gRPC 客户端的入口点和逻辑。


# 总结
## 服务端：
>Nuget包:Grpc.AspNetCore(创建grpc服务会自动创建)
>创建proto文件,例如使用模版生成时自带的`greet.proto`
```proto
    syntax = "proto3"; //指定使用pb3,否则会是默认为proto2

    option csharp_namespace = "CoreDemo_GrpcService"; //命名空间 可空

    package Greet; //包 可空

    // The greeting service definition. 定义服务
    service Greeter {
        // Sends a greeting
        rpc SayHello (HelloRequest) returns (HelloReply); //返回HelloReply对象
    }
    //两个消息对象定义
    // The request message containing the user's name.
    message HelloRequest {
    string name = 1;
    }
    // The response message containing the greetings.
    message HelloReply {
    string message = 1;
    }
```
>`dotnet build` 编译，默认会在`.csproj`文件夹下生成
```xml
     <Protobuf Include="Protos\user.proto">
        <GrpcServices>Server</GrpcServices>
    </Protobuf>
```
>如果没有则添加` <Protobuf Include="Protos\user.proto" GrpcServices="Service"/>`
>编写Service服务:创建Services目录，添加`XxxService.cs`,继承Proto文件电仪的Service.ServiceBase,比如`Greeter.GreeterBase`
>重写Service方法：比如重写SayHello方法`public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context){...}`
>重新生成，编译通过则完成
## 客户端
>Nuget:Google.Protobuf,Grpc.Net.Client,Grpc.Tools
>复制Service的Protos文件到本项目
>重新生成：此时会生成类的同时会在`.csproj`中添加`  <Protobuf Include="Protos\user.proto"><GrpcServices>Server</GrpcServices></Protobuf>` 移除它并且添加`<Protobuf Include="Protos\user.protp" GrpServices="Client"`,重新生成编译通过
>调用服务端的接口
```csharp
    var channel = GrpcChannel.ForAddress("https://localhost:5001");
    var client = new Greeter.GreeterClient(channel);
    var  reply= await client.SayHelloAsync(new HelloRequest { Name = "weixiao" });
```
# gRPC 流
gRPC 有四种服务类型，分别是：简单 RPC（Unary RPC）、服务端流式 RPC （Server streaming RPC）、客户端流式 RPC （Client streaming RPC）、双向流式 RPC（Bi-directional streaming RPC）
## 简单 RPC	一般的rpc调用，传入一个请求对象，返回一个返回对象
>服务端
```csharp
 public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        {

            return Task.FromResult(new HelloReply
            {
                Message = "Hello " + request.Name
            });
        }
```
>客户端
```csharp
    var channel = GrpcChannel.ForAddress("https://localhost:5001");
    var client = new Greeter.GreeterClient(channel);
    var  reply= await client.SayHelloAsync(new HelloRequest { Name = "weixiao" });
    Console.WriteLine("Greeter 服务返回数据: " + reply.Message);
```
## 服务端流式 RPC	传入一个请求对象，服务端可以返回多个结果对象
>服务端
```csharp
 public override async Task<HelloReply> SayHello1(IAsyncStreamReader<HelloRequest> requestStream, ServerCallContext context)
        {
             var stopwatch = new Stopwatch();
            stopwatch.Start();
            //var x = requestStream.Current;//这样读取会报错
            while (await requestStream.MoveNext())
            {
                Console.WriteLine(requestStream.Current.Name);
            }
            stopwatch.Stop();
            var timer=stopwatch.ElapsedMilliseconds / 1000;
            return new HelloReply(){Message="Hi,"+requestStream.Current.Name+"![服务端流式grpc]"+timer.ToString()};
            // return base.SayHello1(requestStream, context);
        }
```
>客户端
```csharp
    var channel = GrpcChannel.ForAddress("https://localhost:5001");
    var client = new Greeter.GreeterClient(channel);
    var r1 = client.SayHello1();
        for (int i = 0; i < 10; i++)
        {
            await r1.RequestStream.WriteAsync(new HelloRequest(){Name="weixiao"+i});
        }
        await r1.RequestStream.CompleteAsync();
```
## 客户端流式 RPC	客户端传入多个请求对象，服务端返回一个结果对象
>服务端
```csharp
 public override async Task SayHello2(HelloRequest request, IServerStreamWriter<HelloReply> responseStream, ServerCallContext context)
        {
            for (int i = 0; i < 4; i++)
            {
                 await responseStream.WriteAsync(new HelloReply(){Message="Hi,"+request.Name+"![客户端流式grpc]"+i});
            }
            //return base.SayHello2(request, responseStream, context);
        }
```
>客户端
```csharp
    var channel = GrpcChannel.ForAddress("https://localhost:5001");
    var client = new Greeter.GreeterClient(channel);
    using (var r2=client.SayHello2(new HelloRequest(){Name="weixiao"}))
    {
        var iterator = r2.ResponseStream;
        // var x = r2.ResponseStream.Current.Message; //这么读报错
        //Console.WriteLine(x);
        while (await iterator.MoveNext())
        {
            Console.WriteLine(iterator.Current.Message);
        }
     }

```
## 双向流式 RPC	结合客户端流式RPC和服务端流式RPC，可以传入多个请求对象，返回多个结果对象
>服务端
```csharp
 public override async Task SayHello3(IAsyncStreamReader<HelloRequest> requestStream, IServerStreamWriter<HelloReply> responseStream, ServerCallContext context)
        {
            while (await requestStream.MoveNext())
            {
                 Console.WriteLine(requestStream.Current.Name);
                 await responseStream.WriteAsync(new HelloReply(){Message="Hi,"+requestStream.Current.Name+"![双流]"});
            }
            // return base.SayHello3(requestStream, responseStream, context);
        }
```
>客户端
```csharp
    var channel = GrpcChannel.ForAddress("https://localhost:5001");
    var client = new Greeter.GreeterClient(channel);
    Console.WriteLine("双向流式1 RPC ");
    // // 双向流式 1  RPC
    using (var r3=client.SayHello3())
    {
        await r3.RequestStream.WriteAsync(new HelloRequest(){Name="wx1"});
        await r3.RequestStream.WriteAsync(new HelloRequest(){Name="wx2"});
        await r3.RequestStream.WriteAsync(new HelloRequest(){Name="wx3"});
        var iterator = r3.ResponseStream;
        await r3.RequestStream.CompleteAsync();
        while (await iterator.MoveNext())
        {
            Console.WriteLine(r3.ResponseStream.Current.Message);
        }
    }
     Console.WriteLine("双向流式2 RPC ");
     // // 双向流式 2  RPC
    var r4 = client.SayHello3();
    for (int i = 0; i < 3; i++)
    {
        await r4.RequestStream.WriteAsync(new HelloRequest(){Name="wx"+i});
    }
    await r4.RequestStream.CompleteAsync();
    await foreach (var resp in r4.ResponseStream.ReadAllAsync())
    {
        Console.WriteLine(resp.Message);
    }
```
## 流控制
gRPC 的流式调用支持对流进行主动取消的控制，进而可以衍生出流超时限制等控制。
在流式调用是，可以传一个 CancellationToken 参数，它就是我们用来对流进行取消控制的
>客户端
```csharp
     var cts = new CancellationTokenSource();
            //指定在2.5s后进行取消操作
            cts.CancelAfter(TimeSpan.FromSeconds(2.5));
            try{
                var r5 = client.SayHello3(cancellationToken:cts.Token);
                for (int i = 0; i < 100; i++)
                {
                    await r5.RequestStream.WriteAsync(new HelloRequest(){Name="wx"+i});
                }
                await r5.RequestStream.CompleteAsync();

                await foreach (var resp in r5.ResponseStream.ReadAllAsync())
                {
                        Console.WriteLine(resp.Message);
                }
            }
            catch (RpcException ex)  when (ex.StatusCode == StatusCode.Cancelled)
                {
                     Console.WriteLine("Stream cancelled.");
                }
            catch(Exception ex){
                    Console.WriteLine(ex.Message);
        }
```
#  gRPC 拦截器
拦截器就像MVC的过滤器或者是ASP.NET Core middleware 一样，具有面向切面的思想，可以在调用服务的时候进行一些统一处理， 很适合在这里处理验证、日志等流程。
## Interceptor 类介绍
方法名称	作用
BlockingUnaryCall	拦截阻塞调用
AsyncUnaryCall	拦截异步调用
AsyncServerStreamingCall	拦截异步服务端流调用
AsyncClientStreamingCall	拦截异步客户端流调用
AsyncDuplexStreamingCall	拦截异步双向流调用
UnaryServerHandler	用于拦截和传入普通调用服务器端处理程序
ClientStreamingServerHandler	用于拦截客户端流调用的服务器端处理程序
ServerStreamingServerHandler	用于拦截服务端流调用的服务器端处理程序
DuplexStreamingServerHandler	用于拦截双向流调用的服务器端处理程序
### 客户端拦截器
>创建类`ClientLoggerInterceptor.cs` 继承拦截器基类`Interceptor`,重写对应的方法
>注册拦截器
```csharp
    var channel = GrpcChannel.ForAddress("https://localhost:5001");
    var invoker = channel.Intercept(new ClientLoggerInterceptor()); //注册
    var clientIv = new Greeter.GreeterClient(invoker);
    var  replys= await clientIv.SayHelloAsync(new HelloRequest { Name = "weixiao" });
    Console.WriteLine("Greeter 服务返回数据: " + replys.Message);
```
### 服务端拦截器
>创建类`ServerLoggerInterceptor.cs`  继承拦截器基类`Interceptor`,重写对应的方法
>注册拦截器
```csharp

```
