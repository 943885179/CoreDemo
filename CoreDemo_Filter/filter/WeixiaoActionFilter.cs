using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreDemo_Filter.filter
{
    public class WeixiaoActionFilter : IActionFilter
    {
        /// <summary>
        /// 执行后
        /// </summary>
        /// <param name="context"></param>
        public void OnActionExecuted(ActionExecutedContext context)
        {
            Console.WriteLine("IActionFilter  执行后拦截");
        }
        /// <summary>
        /// 执行前
        /// </summary>
        /// <param name="context"></param>
        public void OnActionExecuting(ActionExecutingContext context)
        {
            Console.WriteLine("IActionFilter 执行前拦截");
        }
    }
}
