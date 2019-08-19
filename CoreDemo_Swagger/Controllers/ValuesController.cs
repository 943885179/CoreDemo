using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoreDemo_Swagger.Controllers
{
    /// <summary>
    /// 测试
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        /// <summary>
        /// 测试Get
        /// </summary>
        /// <returns></returns>
        // GET api/values
        [HttpGet]
        [AllowAnonymous]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }
        /// <summary>
        /// 测试get path
        /// </summary>
        /// <param name="id">编号</param>
        /// <remarks>当前方法根据ID获取一个值</remarks>
        /// <returns></returns>
        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            var x = HttpContext.Request;
            var arifExpr = CalcByDataTable("2 * (2 + 3)");
            string[] text = { "Albert was here", "Burke slept late", "Connor is happy" };

            var tokens = text.Select(s => s.Split(' '));

            foreach (string[] line in tokens)
            {

                foreach (string token in line)
                {
                    Console.Write("{0}.", token);
                }
            }



            string[] texts = { "Albert was here", "Burke slept late", "Connor is happy" };
            var tokenss = text.SelectMany(s => s.Split(' '));
            foreach (string token in tokenss)
            {
                Console.Write("{0}.", token);

            }
            return id.ToString();
        }
        /// <summary>
        /// 由DataTable计算公式
        /// </summary>
        /// <param name="expression">表达式</param>
        internal static float CalcByDataTable(string expression)
        {
            object result = new DataTable().Compute(expression, "123");
            return float.Parse(result + "");
        }
        /*private object EvalExpress(string sExpression)
        {
            Microsoft.JScript.Vsa.VsaEngine ve = Microsoft.JScript.Vsa.VsaEngine.CreateEngine();
            return Microsoft.JScript.Eval.JScriptEvaluate(sExpression, ve);
        }*/
        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
