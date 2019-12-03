using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreDemo_Filter.filter
{
    public class WeixiaoAsyncActionFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            Console.WriteLine("异步路由执行");
            await next.Invoke();
            await this.OnExecutedAsync(context);
        }
        /// <summary>
        /// 执行后
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task OnExecutedAsync(ActionExecutingContext context)
        {
            Console.WriteLine("异步路由执行后");
            await Task.CompletedTask;
        }
    }
}
