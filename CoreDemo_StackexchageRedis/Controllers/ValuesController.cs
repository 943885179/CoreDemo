using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;

namespace CoreDemo_StackexchageRedis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private IConfiguration _config;
        private IDatabase redisDataBase;
        public ValuesController(IConfiguration config)
        {
            _config = config;
            redisDataBase = RedisClientSingleton.GetInstance(_config).GetDatabase("Redis_Default");
        }
        /// <summary>
        /// 添加string类型
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>

        [HttpGet("StringSet/{key}/{value}")]
        public ActionResult<bool> StringSet(string key, string value)
        {
            return redisDataBase.StringSet(key, value);
        }
        /// <summary>
        /// 添加string类型
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpGet("StringGet/{key}")]
        public async Task<string> StringGet(string key)
        {
            return await  redisDataBase.StringGetAsync(key);
        }

    }
}
