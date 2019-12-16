# 下载对应的安装包[链接](https://github.com/google/protobuf/releases)
# 将Protoc.exe加入环境变量,例如解压到`E:\protocbuf_win64` 则添加环境变量为``E:\protocbuf_win64\bin`
# cmd 输入`protoc --version`则可以查看是否安装成功
>`protoc --version` 查看版本
>`protoc -h` 查看帮助
## java 生成
` protoc --java_out=生成地址  protoc文件地址`  例如`protoc  --java_out=./ ./protos/greet.proto`

## go生成： protoc --go_out=plugins=grpc:{输出目录}  {proto文件}：必须保证输出目录存在
进入GOPATH目录 运行`go get -u github.com/golang/protobuf/protoc-gen-go` GOPATH/bin下生成protoc-gen-go.exe文件
>前置：需要将`GOPATH/bin`加入环境变量
>下载protobuf的`include`文件夹下的内容`google`文件复制到`GOPATH/bin`下，否则无法调用常用的`google/protobuf/empty.proto`等
`$ protoc --go_out=./go/ ./proto/helloworld.proto` 生成go

## csharp生成
` protoc --csharp_out=生成地址  protoc文件地址`  例如`protoc  --java_out=./ ./protos/greet.proto`

## 文件批量生成 `protoc --go_out=./ *.proto` 需要包名统一，否则报错
