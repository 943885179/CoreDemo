# Token添加
## 修改appsetting.json
 ```json
    {
    "Logging": {
        "IncludeScopes": false,
        "Debug": {
        "LogLevel": {
            "Default": "Warning"
        }
        },
        "Console": {
        "LogLevel": {
            "Default": "Warning"
        }
        }
    },

    "JwtSettings": {
        "Issuer": "weixiao",
        "Audience": "mzj",
        "SecretKey": "weixiaomzj666888xxxzzzz"
    }
    }
 ```
## 添加token帮助类`JwtSettings.cs`
 ```csharp
    public class JwtSettings
         {
            //token是谁颁发的
            public string Issuer { get; set; }
            //token可以给哪些客户端使用
            public string Audience { get; set; }
            //加密的key
            public string SecretKey { get; set; }
        }
 ```
## 注册
 ```csharp
    public void ConfigureServices(IServiceCollection services)
        {//将appsettings.json中的JwtSettings部分文件读取到JwtSettings中，这是给其他地方用的
            services.Configure<JwtSettings>(Configuration.GetSection("JwtSettings"));

            //由于初始化的时候我们就需要用，所以使用Bind的方式读取配置
            //将配置绑定到JwtSettings实例中
            var jwtSettings = new JwtSettings();
            Configuration.Bind("JwtSettings", jwtSettings);

            services.AddAuthentication(options => {
                //认证middleware配置
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(o => {
                //主要是jwt  token参数设置
                o.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    //Token颁发机构
                    ValidIssuer = jwtSettings.Issuer,
                    //颁发给谁
                    ValidAudience = jwtSettings.Audience,
                    //这里的key要进行加密，需要引用Microsoft.IdentityModel.Tokens
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey))
                    //ValidateIssuerSigningKey=true,
                    ////是否验证Token有效期，使用当前时间与Token的Claims中的NotBefore和Expires对比
                    //ValidateLifetime=true,
                    ////允许的服务器时间偏移量
                    //ClockSkew=TimeSpan.Zero

                };
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }
 ```
 `app.UseAuthentication();`
## 创建登录实体 
 ```csharp
   public class LoginViewModel
    {
        //用户名
        [Required]
        public string User { get; set; }
        //密码
        [Required]
        public string Password { get; set; }
    }
 ```
## 创建登录接口（生成token）
 ```csharp
   [HttpPost("Token")]
        public IActionResult Token(LoginViewModel viewModel)
        {
            if (ModelState.IsValid)//判断是否合法
            {
                if (!(viewModel.User == "admin" && viewModel.Password == "123456"))//判断账号密码是否正确
                {
                    return BadRequest();
                }
                var claim = new Claim[]{ //声明
                    new Claim(ClaimTypes.Name,"weixiaoqaq"),
                    new Claim(ClaimTypes.Role,"admin")
                };

                //对称秘钥
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
                //签名证书(秘钥，加密算法)
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                //生成token  [注意]需要nuget添加Microsoft.AspNetCore.Authentication.JwtBearer包，并引用System.IdentityModel.Tokens.Jwt命名空间
                var token = new JwtSecurityToken(_jwtSettings.Issuer, _jwtSettings.Audience, claim, DateTime.Now, DateTime.Now.AddMinutes(30), creds);

                return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });

            }

            return BadRequest();
        }
 ```
## 其他接口添加权限认证 [ApiController] 和[AllowAnoymous]
 ```csharp
  
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] //添加token验证
    public class ValuesController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        [AllowAnonymous]//跳过验证
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }
    }
 ```
