>$ cd $GOPATH/src/google.golang.org
>git clone https://github.com/grpc/grpc-go.git grpc
>go install google.golang.org/grpc

# 安装grpc
## 官方安装:`go get -u google.glang.org/grpc` 需要能访问google
## 手动安装：
> `git clone  https://github.com/grpc/grpc-go.git $GOPATH/src/google.golang.org/grpc`
> `git clone https://github.com/golang/net.git $GOPATH/src/golang.org/x/net`
> `git clone https://github.com/golang/text.git $GOPATH/src/golang.org/x/text`
> `go get -u github.com/golang/protobuf/protoc-gen-go`
> `git clone https://github.com/google/go-genproto.git $GOPATH/src/google.golang.org/genproto`
> `cd $GOPATH/src/`
> `go install google.golang.org/grpc`
## 生成grpc服务代码编译器: `go get -u github.com/golang/protobuf/protoc-gen-go`
# 编译.proto文件
>`protoc --go_out=plugins=grpc:. x.proto`
