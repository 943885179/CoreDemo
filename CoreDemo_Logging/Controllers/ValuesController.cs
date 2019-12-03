using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CoreDemo_Logging.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        ILogger<ValuesController> logger;
        //构造函数注入Logger
        public ValuesController(ILogger<ValuesController> logger)
        {
            this.logger = logger;
        }
        [HttpGet]
        public IEnumerable<string> Get()
        {
            logger.LogWarning("Warning");
            logger.LogError("Error");
            logger.LogDebug("Debug");
            //方法二：生成logger
            var loggerNew = new LoggerFactory().AddLog4Net().CreateLogger("Logs");
            loggerNew.LogError($"{DateTime.Now}错误");
            return new string[] { "value1", "value2" };
        }
    }
}
