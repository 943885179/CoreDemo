using System;
using System.Threading;
using System.Threading.Tasks;
using CoreDemo_GrpcService;
using Grpc.Core;
using Grpc.Core.Interceptors;
using Grpc.Net.Client;
using static CoreDemo_GrpcService.UserType.Types;
namespace CoreDemo_GrpsClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var invoker = channel.Intercept(new ClientLoggerInterceptor());
            var clientIv = new Greeter.GreeterClient(invoker);
            var  replys= await clientIv.SayHelloAsync(new HelloRequest { Name = "weixiao" });
             Console.WriteLine("Greeter 服务返回数据: " + replys.Message);


            var client = new Greeter.GreeterClient(channel);
             //简单gRpc
             var  reply= await client.SayHelloAsync(new HelloRequest { Name = "weixiao" });
             Console.WriteLine("Greeter 服务返回数据: " + reply.Message);
            // //服务器端流式 RPC
             Console.WriteLine("服务器端流式 RPC ");
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
             Console.WriteLine("客户端流 RPC ");
            // //客户端流 RPC
            var r1 = client.SayHello1();
            for (int i = 0; i < 10; i++)
            {
                await r1.RequestStream.WriteAsync(new HelloRequest(){Name="weixiao"+i});
            }
            await r1.RequestStream.CompleteAsync();

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
             Console.WriteLine("流控制");
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

            var uclient=new User.UserClient(channel);
            var repu = await uclient.GetUserAsync(new UserRequest(){Message="你好啊"});
            Console.WriteLine(repu.Name+":" + repu.Age);
            UserType obj=new UserType();
            obj.Phones.Add(item: new UserType.Types.PhoneNumber() { Number = "18206840781",Type=PhoneType.Mobile });
            obj.Phones.Add(item: new UserType.Types.PhoneNumber() { Number = "18206840782",Type=PhoneType.Mobile });
            obj.Skills.Add("Test", new Skill() { Name = "weixiao" });
            var ty = await uclient.GetUserTypeAsync(obj);
            foreach (var item in ty.Phones)
            {
                Console.WriteLine(item.Number + ":" + item.Type);
            }
            foreach (var item in ty.Skills)
            {

                Console.WriteLine(item.Key + ":" + item.Value.Name);
            }
            Console.ReadKey();
        }
    }
}
