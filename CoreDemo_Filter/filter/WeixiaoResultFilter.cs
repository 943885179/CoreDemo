using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreDemo_Filter.filter
{
    public class WeixiaoResultFilter : IResultFilter
    {
        /// <summary>
        /// 结果返回后
        /// </summary>
        /// <param name="context"></param>
        public void OnResultExecuted(ResultExecutedContext context)
        {
            Console.WriteLine("动作结果执行后");
        }
        /// <summary>
        /// 结果返回前
        /// </summary>
        /// <param name="context"></param>
        public void OnResultExecuting(ResultExecutingContext context)
        {
            Console.WriteLine("动作结果执行后");
        }
    }
}
