using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace CoreDemo_GrpcService.Services
{
    public class GreeterService : Greeter.GreeterBase
    {
        private readonly ILogger<GreeterService> _logger;

        public GreeterService(ILogger<GreeterService> logger)
        {
            _logger = logger;
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        {

            return Task.FromResult(new HelloReply
            {
                Message = "Hello " + request.Name
            });
        }
        //客户端流 RPC
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
        //服务器端流式 RPC
        public override async Task SayHello2(HelloRequest request, IServerStreamWriter<HelloReply> responseStream, ServerCallContext context)
        {
            for (int i = 0; i < 4; i++)
            {
                 await responseStream.WriteAsync(new HelloReply(){Message="Hi,"+request.Name+"![客户端流式grpc]"+i});
            }
            //return base.SayHello2(request, responseStream, context);
        }

        public override async Task SayHello3(IAsyncStreamReader<HelloRequest> requestStream, IServerStreamWriter<HelloReply> responseStream, ServerCallContext context)
        {
            while (await requestStream.MoveNext())
            {
                 Console.WriteLine(requestStream.Current.Name);
                 await responseStream.WriteAsync(new HelloReply(){Message="Hi,"+requestStream.Current.Name+"![双流]"});
                 await Task.Delay(500);//此处主要是为了方便客户端能看出流调用的效果
            }
            // return base.SayHello3(requestStream, responseStream, context);
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
