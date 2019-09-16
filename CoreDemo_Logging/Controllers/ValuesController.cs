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
            return new string[] { "value1", "value2" };
        }
    }
}
