# 创建三个项目 `Api`、`QuickstartIdentityServer`、`Client`;
## 客户端调用模式 :Client Credentials Grant （客户端模式）是Oauth2.0协议中，四种模式自建单的一种。它由两部分构成，客户端和认证服务器。认证服务器确认客户端无误后返回一个token，客户端请求带着token访问资源。（一般使用场景是在一个安全的环境下，例如我的同一个系统中，一个api请求另外一个api）
### QuickstartIdentityServer
  - 添加Nuget `IdentityServer4`;
  - 设置启动地址`  WebHost.CreateDefaultBuilder(args).UseUrls("http://localhost:5000;https://localhost:5001")`;
  - 添加配置文件`Config.cs`;
    ```csharp
        public static class Config
        {
            public static IEnumerable<IdentityResource> GetIdentityResources()
            {
                return new IdentityResource[]
                {
                    new IdentityResources.OpenId()
                };
            }
            //定义 API
            public static IEnumerable<ApiResource> GetApis()
            {
                return new List<ApiResource>
                {
                    new ApiResource("api1", "My API")
                };
            }
            //定义 客户端
            public static IEnumerable<Client> GetClients()
            {
                return new List<Client>
                {
                    new Client
                    {
                        ClientId = "client",
                        // 没有交互性用户，使用 clientid/secret 实现认证。
                        AllowedGrantTypes = GrantTypes.ClientCredentials,//设置模式，客户端模式
                        //用于认证的密码
                        ClientSecrets =
                        {
                            new Secret("secret".Sha256())
                        },

                        // scopes that client has access to
                        //客户端有权访问的范围（Scopes）
                        AllowedScopes = { "api1" }
                    }
                };
            }
        }
    ```
 - 注入DI `startuo.cs`
    ```csharp
         services.AddIdentityServer()
                .AddDeveloperSigningCredential()
                .AddInMemoryIdentityResources(Config.GetIdentityResources())
                .AddInMemoryApiResources(Config.GetApis())
                .AddInMemoryClients(Config.GetClients()); 
    ```
    ` app.UseIdentityServer();`
 - 启动项目查看是否成功（会出现你所有的配置信息） [https://localhost:5001/.well-known/openid-configuration](https://localhost:5001/.well-known/openid-configuration)
 
 - 上一步有个`token_endpoint`,postman调用可以获取token，调用方式post,url:"https://localhost:5001/connect/token",from-data参数
    |参数名称|参数值|备注|
    |--|--|--|
    |client_id|client|config.cs配置的ClientId|
    |client_secret|secret|config.cs配置的ClientSecrets|
    |grant_type|client_credentials|grant_types_supported类型|
### 配置API接口项目
 - Nuget `IdentityServer4.AccessTokenValidation` 也可以不用（下面没有用到）
 - 添加权限认证
  ```csharp
  services.AddAuthentication("Bearer")
                .AddCookie("Cookies")
                .AddJwtBearer("Bearer", options =>
                {
                    //identityserver4 地址 也就是本项目地址
                    options.Authority = "https://localhost:5001"; //配置Identityserver的授权地址
                    options.RequireHttpsMetadata = true; //不需要https    
                    options.Audience = "api1";   //api的name，需要和config的名称相同
                });
  ```
  `app.UseAuthentication();// 添加认证中间件 `
  - 接口添加[Authorize] 某个接口不用则用[AllowAnonymous]
### client端编写（前面通过postman获取token后就可以直接访问接口了，不过这个适合写在程序里面请求接口）
- Nuget `IdentityModel`
- 获取IdentityServer的配置信息
  ```csharp
      [HttpGet]
        public async Task<object> GetAsync()
        {
            var client = new HttpClient();
            var disco = await client.GetDiscoveryDocumentAsync("https://localhost:5001");//这个是IdentityServer地址
            if (disco.IsError)
            {
                 return disco.Error;
            }
            return disco;
        }
  ```
 - 获取Token
  ```csharp
      [HttpGet]
        public async Task<object> GetAsync()
        {
            var client = new HttpClient();
            var disco = await client.GetDiscoveryDocumentAsync("https://localhost:5001");
            if (disco.IsError)
            {
                 return disco.Error;
            }
            var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = disco.TokenEndpoint,

                ClientId = "client",//和Config.cs的ClientId一致
                ClientSecret = "secret",//和Config.cs的ClientSecret一致
                Scope = "api1"//和Config.cs的Scope一致
            });

            if (tokenResponse.IsError)
            {
               return tokenResponse.Error;
            }
           return tokenResponse.Json;
        }
  ```
 - 获取Api接口返回值
  ```csharp
      [HttpGet]
        public async Task<object> GetAsync()
        {
           var clients = new HttpClient();
            clients.SetBearerToken(tokenResponse.AccessToken);//tokenResponse.AccessToken 从上一个token获取接口得到

            var response = await clients.GetAsync("https://localhost:5005/api/identity");//接口实际地址
            if (!response.IsSuccessStatusCode)
            {
                var s = response.StatusCode;
                return response.StatusCode;
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync();
                return content;
            }
        }
  ```
## 密码模式 用户会将用户名，密码给予客户端，但是客户端不保存此信息，客户端带着用户的密码请求认证服务器，认证服务器密码验证通过后后将token返回给客户端。
 使用上面的代码所以调整不会很大
### `Config.cs`添加一个新的客户端
 ```csharp
    new Client()
                {
                    ClientId="PasswordClient",
                    AllowedGrantTypes=GrantTypes.ResourceOwnerPassword,
                    ClientSecrets={ new Secret("secretPasswordxxxxx".Sha256())},
                    AllowedScopes={ "api1"}
                }
 ```
### `Config.cs`添加用户
 ```csharp
   public static List<TestUser> GetTestUsers() {
            return new List<TestUser>() {
                new TestUser()
                {
                    SubjectId="1",
                    Username="weixiao",
                    Password="123"
                }
            };
        }
 ```
### 用户注入
 ```csharp
   services.AddIdentityServer()//赖注入系统中注册IdentityServer,注入DI
                .AddDeveloperSigningCredential()//在每次启动时，为令牌签名创建了一个临时密钥。在生成环境需要一个持久化的密钥
                .AddInMemoryIdentityResources(Config.GetIdentityResources())
                .AddInMemoryApiResources(Config.GetApis())
                .AddInMemoryClients(Config.GetClients())
                .AddTestUsers(Config.GetTestUsers()); //用户添加
 ```

### PostMan调用接口获取token 调用方式post,url:"https://localhost:5001/connect/token",from-data参数
|参数名称|参数值|备注|
|--|--|--|
|client_id|passwordClient|config.cs配置的ClientId|
|client_secret|secretPasswordxxxxx|config.cs配置的ClientSecrets|
|grant_type|password|grant_types_supported类型|
|userName|weixiao|配置的姓名|
|password|123|密码|
### 根据token 获取接口数据 Authorization配置为Bearer Token 放入token获取
### Client端调用获取token有些区别
 ```csharp
     [HttpGet]
        public async Task<object> GetAsync()
        {
            var client = new HttpClient();
            var disco = await client.GetDiscoveryDocumentAsync("https://localhost:5001");
            if (disco.IsError)
            {
                 return disco.Error;
            }
           var tokenResponse = await client.RequestPasswordTokenAsync(new PasswordTokenRequest() {

                Address = disco.TokenEndpoint,
                ClientId= "PasswordClient",
                ClientSecret= "secretPasswordxxxxx",
                UserName="weixiao",
                Password="123",
                Scope="api1"
            });
            if (tokenResponse.IsError)
            {
               return tokenResponse.Error;
            }
           return tokenResponse.Json;
        }
 ```
## Oauth2协议授权码模式介绍
 授权码模式是Oauth2协议中最严格的认证模式，它的组成以及运行流程是这样
 - 1、用户访问客户端，客户端将用户导向认证服务器
 - 2、用户在认证服务器输入用户名密码选择授权，认证服务器认证成功后，跳转至一个指定好的"跳转Url"，同时携带一个认证码。
 - 3、用户携带认证码请求指定好的"跳转Url"再次请求认证服务器（这一步后台完成，对用户不可见），此时，由认证服务器返回一个Token
 - 4、客户端携带token请求用户资源