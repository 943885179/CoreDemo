using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace CoreDemo_Authentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizeController : Controller
    {
        private JwtSettings _jwtSettings;

        public AuthorizeController(IOptions<JwtSettings> _jwtSettingsAccesser)
        {
            _jwtSettings = _jwtSettingsAccesser.Value;
        }
        [HttpGet]
        public string Get()
        {
            return "ada";
        }
        [HttpPost]
        public string Post()
        {
            return "Post";
        }
        [HttpPost("Token")]
        public IActionResult Token(LoginViewModel viewModel)
        {
            if (ModelState.IsValid)//判断是否合法
            {
                if (!(viewModel.User == "admin" && viewModel.Password == "123456"))//判断账号密码是否正确
                {
                    return BadRequest();
                }
                var claim = new Claim[]{
                    new Claim(ClaimTypes.Name,"admin"),
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
    }
}