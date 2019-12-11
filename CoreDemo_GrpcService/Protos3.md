# 语法介绍
## `syntax = "proto3";`:文件的第一行指定了你正在使用proto3语法：如果你没有指定这个，编译器会使用proto2。这个指定语法行必须是文件的非空非注释的第一个行。
## `message XxxRequest{ type name = 1; }`:指定消息格式，里面为参数，定义格式为`类型 名称 = 标识号`例如`Message SeachRequst{ int id=1;string name=2;}`
### 字段类型:string ,int ,emum 等
### 标识号：在消息定义中，每个字段都有唯一的一个数字标识符。这些标识符是用来在消息的二进制格式中识别各个字段的，一旦开始使用就不能够再改变。注：[1,15]之内的标识号在编码的时候会占用一个字节。[16,2047]之内的标识号则占用2个字节。所以应该为那些频繁出现的消息元素保留 [1,15]之内的标识号。切记：要为将来有可能添加的、频繁出现的标识号预留一些标识号。最小的标识号可以从1开始，最大到2^29 - 1, or 536,870,911。不可以使用其中的[19000－19999]（ (从FieldDescriptor::kFirstReservedNumber 到 FieldDescriptor::kLastReservedNumber)）的标识号， Protobuf协议实现中对这些进行了预留。如果非要在.proto文件中使用这些预留标识号，编译时就会报警。同样你也不能使用早期保留的标识号。
### 字段规则:
> singular：一个格式良好的消息应该有0个或者1个这种字段（但是不能超过1个）
> repeated：在一个格式良好的消息中，这种字段可以重复任意多次（包括0次）。重复的值的顺序会被保留。
> 在proto3中，repeated的标量域默认情况虾使用packed。
## 注释 可以使用C/C++/java风格的双斜杠（//） 语法格式
## 保留标识符（Reserved）不要在同一行reserved声明中同时声明域名字和标识号
```
    message Foo {
    reserved 2, 15, 9 to 11;
    reserved "foo", "bar";
    }
```
从.proto文件生成了什么？
当用protocol buffer编译器来运行.proto文件时，编译器将生成所选择语言的代码，这些代码可以操作在.proto文件中定义的消息类型，包括获取、设置字段值，将消息序列化到一个输出流中，以及从一个输入流中解析消息。

对C++来说，编译器会为每个.proto文件生成一个.h文件和一个.cc文件，.proto文件中的每一个消息有一个对应的类。
对Java来说，编译器为每一个消息类型生成了一个.java文件，以及一个特殊的Builder类（该类是用来创建消息类接口的）。
对Python来说，有点不太一样——Python编译器为.proto文件中的每个消息类型生成一个含有静态描述符的模块，，该模块与一个元类（metaclass）在运行时（runtime）被用来创建所需的Python数据访问类。
对go来说，编译器会位每个消息类型生成了一个.pd.go文件。
对于Ruby来说，编译器会为每个消息类型生成了一个.rb文件。
javaNano来说，编译器输出类似域java但是没有Builder类
对于Objective-C来说，编译器会为每个消息类型生成了一个pbobjc.h文件和pbobjcm文件，.proto文件中的每一个消息有一个对应的类。
对于C#来说，编译器会为每个消息类型生成了一个.cs文件，.proto文件中的每一个消息有一个对应的类
```
    message SearchRequest {
        string query = 1;
        int32 page_number = 2;
        int32 result_per_page = 3;
        enum Corpus {
            UNIVERSAL = 0;
            WEB = 1;
            IMAGES = 2;
            LOCAL = 3;
            NEWS = 4;
            PRODUCTS = 5;
            VIDEO = 6;
        }
        Corpus corpus = 4;
        }
```
## 嵌套类型
```
message SearchResponse {
  message Result {
    string url = 1;
    string title = 2;
    repeated string snippets = 3;
  }
  repeated Result results = 1;
}
```
## Any
Any类型消息允许你在没有指定他们的.proto定义的情况下使用消息作为一个嵌套类型。一个Any类型包括一个可以被序列化bytes类型的任意消息，以及一个URL作为一个全局标识符和解析消息类型。为了使用Any类型，你需要导入import google/protobuf/any.proto
```
import "google/protobuf/any.proto";

message ErrorStatus {
  string message = 1;
  repeated google.protobuf.Any details = 2;
}
```
## Oneof
如果你的消息中有很多可选字段， 并且同时至多一个字段会被设置， 你可以加强这个行为，使用oneof特性节省内存.

Oneof字段就像可选字段， 除了它们会共享内存， 至多一个字段会被设置。 设置其中一个字段会清除其它字段。 你可以使用case()或者WhichOneof() 方法检查哪个oneof字段被设置， 看你使用什么语言了.
```
message SampleMessage {
  oneof test_oneof {
    string name = 4;
    SubMessage sub_message = 9;
  }
}
```
## Map（映射）
如果你希望创建一个关联映射，protocol buffer提供了一种快捷的语法：
map<key_type, value_type> map_field = N;
## 包 可选的package声明符
```
package foo.bar;
message Open { ... }
```
在其他的消息格式定义中可以使用包名+消息名的方式来定义域的类型，如：
```
message Foo {
  ...
  required foo.bar.Open open = 1;
  ...
}
```
## 定义服务(Service)
如果想要将消息类型用在RPC(远程方法调用)系统中，可以在.proto文件中定义一个RPC服务接口，protocol buffer编译器将会根据所选择的不同语言生成服务接口代码及存根。如，想要定义一个RPC服务并具有一个方法，该方法能够接收 SearchRequest并返回一个SearchResponse，此时可以在.proto文件中进行如下定义：
```
service SearchService {
  rpc Search (SearchRequest) returns (SearchResponse);
}
```
## JSON 映射
## 选项 在定义.proto文件时能够标注一系列的options。Options并不改变整个文件声明的含义，但却能够影响特定环境下处理方式。完整的可用选项可以在google/protobuf/descriptor.proto找到。
## 自定义选项
ProtocolBuffers允许自定义并使用选项。该功能应该属于一个高级特性，对于大部分人是用不到的。如果你的确希望创建自己的选项，请参看 Proto2 Language Guide。注意创建自定义选项使用了拓展，拓展只在proto3中可用。
[参照](https://www.cnblogs.com/tohxyblog/p/8974763.html)

##  import "google/protobuf/empty.proto"; 提供的Empty，这个message没有属性，不需要设置Empty message，而且客户端在调用gRPC接口时，不需要构造Empty对象，直接传null即可，减少gRPC接口的这一规定对客户端的影响。`rpc list(google.protobuf.Empty) returns (UserList);`
