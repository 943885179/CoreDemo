using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreDemo_Filter.filter
{
    public class WeixiaoAsyncResourceFilter : IAsyncResourceFilter
    {
        public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
        {
            Console.WriteLine("异步资源拦截执行前");
            //执行前
            await next.Invoke();
            //执行后
            await this.OnExecutedAsync(context);
        }
        /// <summary>
        /// 执行后
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task OnExecutedAsync(ResourceExecutingContext context)
        {
            Console.WriteLine("异步资源拦截执行后");
            await Task.CompletedTask;
        }
    }
}
