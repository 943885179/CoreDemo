# 下载dapr
> 打开`powershell`
> `powershell -Command "iwr -useb https://raw.githubusercontent.com/dapr/cli/master/install/install.ps1 | iex"` 此时会在c盘下创建dapr文件夹内保护`dapr.exe`
> 输入`dapr` 查看是否安装成功，添加到Path
> `dapr init ` 初始化，很可能失败，重复多执行几次，翻墙
# 下载helloworld:[github](git clone https://github.com/dapr/samples.git)
>cd samples/1.hello-world
>npm install
>dapr run --app-id nodeapp -app-port 3000 --port 3500 node app.js
此时会提示`Starting Dapr with id nodeapp. HTTP Port: 3500. gRPC Port: 9165
You're up and running! Both Dapr and your app logs will appear here.
...`表示成功了
> post调用：`http://localhost:3500/v1.0/invoke/nodeapp/method/neworder` 参数为`{"data": { "orderId": "41" } }`或者使用powershell命令`dapr invoke --app-id nodeapp --method neworder --payload '{\"data\": { \"orderId\": \"41\" } }'`
提示出`?[0m?[94;1m== APP == Got a new order! Order ID: 42
?[0m?[94;1m== APP == Successfully persisted state.`
>get调用:`http://localhost:3500/v1.0/invoke/nodeapp/method/order` 或者控制台调用`curl http://localhost:3500/v1.0/invoke/nodeapp/method/order`
>停止`dapr stop --app-id nodeapp`
>查看运行中的程序`dapr list`
# 启动.net Core程序：dapr run --app-id CoreApp --app-port 5002 --port 5000  dotnet run
> --app-id 命名
> --app-port dapr监听端口
> --port 系统启动端口（不指定随机）
