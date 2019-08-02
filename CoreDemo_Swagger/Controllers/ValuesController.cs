﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
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
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }
        /// <summary>
        /// 测试get path
        /// </summary>
        /// <param name="id">编号</param>
        /// <returns></returns>
        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            var arifExpr = CalcByDataTable("2 * (2 + 3)");

            return id.ToString();
        }
        /// <summary>
        /// 由DataTable计算公式
        /// </summary>
        /// <param name="expression">表达式</param>
        internal static float CalcByDataTable(string expression)
        {
            object result = new DataTable().Compute(expression, "");
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
