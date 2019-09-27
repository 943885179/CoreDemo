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
## 使用OpenID Connect添加用户身份验证
 - 添加Mvc客户端'servers'，项目名字随意,starup.cs添加授权
   ```csharp
   JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = "Cookies";
                options.DefaultChallengeScheme = "oidc";
            })
            .AddCookie("Cookies")
            .AddOpenIdConnect("oidc", options =>
            {
                options.SignInScheme = "Cookies";

                options.Authority = "https://localhost:5001";
                options.RequireHttpsMetadata = false;

                options.ClientId = "ImpLicitClient";
                options.SaveTokens = true;
            });
   ```
   ` app.UseAuthentication(); app.UseStaticFiles();`
 - 下载基础Ui(也可以自己写)[地址](https://github.com/IdentityServer/IdentityServer4.Quickstart.UI/)
 - 将基础UI放入IdentityServer端，我都重命名了一下
 - 修改授权端Config.cs代码
    ```csharp
    //添加客户端
        new Client()
                {
                    ClientId="ImpLicitClient",
                    AllowedGrantTypes=GrantTypes.Implicit,//OpenID Connect 简化模式（implicit）
                    ClientSecrets={ new Secret("implicitSecrets".Sha256()) },
                    RequireConsent=true,   //用户选择同意认证授权
                    RedirectUris={ "https://localhost:5008/signin-oidc" }, //指定允许的URI返回令牌或授权码(我们的客户端地址)
                    PostLogoutRedirectUris={ "https://localhost:5008/signout-callback-oidc" },//注销后重定向地址 
                    LogoUri="https://ss1.bdstatic.com/70cFuXSh_Q1YnxGkpoWK1HF6hhy/it/u=3298365745,618961144&fm=27&gp=0.jpg",
                  ////运行访问的资源
                  AllowedScopes = {                       //客户端允许访问个人信息资源的范围
                      IdentityServerConstants.StandardScopes.Profile,
                      IdentityServerConstants.StandardScopes.OpenId,
                      IdentityServerConstants.StandardScopes.Email,
                      IdentityServerConstants.StandardScopes.Address,
                      IdentityServerConstants.StandardScopes.Phone,
                      "api1"
                  }
    ```
 - 如果授权端不是mvc模式还需要修改支持视图（需要构建登录，注销，授权页）等页面
 - 主要方法介绍
    + AccountController 授权接口
      + 登录
        ```csharp
        [HttpGet]
        public async Task<IActionResult> Login(string returnUrl)
        {
            // 构建登录页面模型
            var vm = await BuildLoginViewModelAsync(returnUrl);

            if (vm.IsExternalLoginOnly)
            {
                // 提供扩展登录服务模型
                return RedirectToAction("Challenge", "External", new { provider = vm.ExternalLoginScheme, returnUrl });
            }

            return View(vm);
        }
        /// <summary>
        /// 用户登录提交
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginInputModel model, string button)
        {
            // check if we are in the context of an authorization request
            var context = await _interaction.GetAuthorizationContextAsync(model.ReturnUrl);

            // the user clicked the "cancel" button
            if (button != "login")
            {
                if (context != null)
                {
                    // if the user cancels, send a result back into IdentityServer as if they 
                    // denied the consent (even if this client does not require consent).
                    // this will send back an access denied OIDC error response to the client.
                    await _interaction.GrantConsentAsync(context, ConsentResponse.Denied);

                    // we can trust model.ReturnUrl since GetAuthorizationContextAsync returned non-null
                    if (await _clientStore.IsPkceClientAsync(context.ClientId))
                    {
                        // if the client is PKCE then we assume it's native, so this change in how to
                        // return the response is for better UX for the end user.
                        return View("Redirect", new RedirectViewModel { RedirectUrl = model.ReturnUrl });
                    }

                    return Redirect(model.ReturnUrl);
                }
                else
                {
                    // since we don't have a valid context, then we just go back to the home page
                    return Redirect("~/");
                }
            }

            if (ModelState.IsValid)
            {
                // validate username/password against in-memory store
                if (_users.ValidateCredentials(model.Username, model.Password))
                {
                    var user = _users.FindByUsername(model.Username);
                    await _events.RaiseAsync(new UserLoginSuccessEvent(user.Username, user.SubjectId, user.Username, clientId: context?.ClientId));

                    // only set explicit expiration here if user chooses "remember me". 
                    // otherwise we rely upon expiration configured in cookie middleware.
                    AuthenticationProperties props = null;
                    if (AccountOptions.AllowRememberLogin && model.RememberLogin)
                    {
                        props = new AuthenticationProperties
                        {
                            IsPersistent = true,
                            ExpiresUtc = DateTimeOffset.UtcNow.Add(AccountOptions.RememberMeLoginDuration)
                        };
                    };

                    // issue authentication cookie with subject ID and username
                    await HttpContext.SignInAsync(user.SubjectId, user.Username, props);

                    if (context != null)
                    {
                        if (await _clientStore.IsPkceClientAsync(context.ClientId))
                        {
                            // if the client is PKCE then we assume it's native, so this change in how to
                            // return the response is for better UX for the end user.
                            return View("Redirect", new RedirectViewModel { RedirectUrl = model.ReturnUrl });
                        }

                        // we can trust model.ReturnUrl since GetAuthorizationContextAsync returned non-null
                        return Redirect(model.ReturnUrl);
                    }

                    // request for a local page
                    if (Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }
                    else if (string.IsNullOrEmpty(model.ReturnUrl))
                    {
                        return Redirect("~/");
                    }
                    else
                    {
                        // user might have clicked on a malicious link - should be logged
                        throw new Exception("invalid return URL");
                    }
                }

                await _events.RaiseAsync(new UserLoginFailureEvent(model.Username, "invalid credentials", clientId:context?.ClientId));
                ModelState.AddModelError(string.Empty, AccountOptions.InvalidCredentialsErrorMessage);
            }

            // something went wrong, show form with error
            var vm = await BuildLoginViewModelAsync(model);
            return View(vm);
        }
        ```
     + 退出 同时要在客户端创建退出方法`return SignOut("Cookies", "oidc");`
        ```csharp
         /// <summary>
        /// Show logout page
        /// </summary>
        //[HttpGet]
        public async Task<IActionResult> Logout(string logoutId)
        {
            // build a model so the logout page knows what to display
            var vm = await BuildLogoutViewModelAsync(logoutId);

            if (vm.ShowLogoutPrompt == false)
            {
                // if the request for logout was properly authenticated from IdentityServer, then
                // we don't need to show the prompt and can just log the user out directly.
                return await Logout(vm);
            }

            return View(vm);
        }
        /// <summary>
        /// Handle logout page postback
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout(LogoutInputModel model)
        {
            // build a model so the logged out page knows what to display
            var vm = await BuildLoggedOutViewModelAsync(model.LogoutId);

            if (User?.Identity.IsAuthenticated == true)
            {
                // delete local authentication cookie
                await HttpContext.SignOutAsync();

                // raise the logout event
                await _events.RaiseAsync(new UserLogoutSuccessEvent(User.GetSubjectId(), User.GetDisplayName()));
            }

            // check if we need to trigger sign-out at an upstream identity provider
            if (vm.TriggerExternalSignout)
            {
                // build a return URL so the upstream provider will redirect back
                // to us after the user has logged out. this allows us to then
                // complete our single sign-out processing.
                string url = Url.Action("Logout", new { logoutId = vm.LogoutId });

                // this triggers a redirect to the external provider for sign-out
                return SignOut(new AuthenticationProperties { RedirectUri = url }, vm.ExternalAuthenticationScheme);
            }
            // 验证返回URL并重定向回授权端点或本地页面
            var returnUrl = result.Properties.Items["returnUrl"];
            if (_interaction.IsValidReturnUrl(returnUrl) || Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return Redirect("~/");
            //return View("LoggedOut", vm);
        }
      ```
   - AccountService.cs 授权服务
## QQ第三方登录（引入Nuget Microsoft.AspNetCore.Authentication.QQ）
- 添加授权
  ```csharp
   JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = "Cookies";
                options.DefaultChallengeScheme = "oidc";
            })
            .AddCookie("Cookies")
            .AddQQ("QQ", qqOptions =>
             {
                 qqOptions.AppId = "<insert here>";
                 qqOptions.AppKey = "<insert here>";
             })
    ```
## 使用Hybrid Flow并添加API访问控制