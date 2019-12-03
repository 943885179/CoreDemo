using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreDemo_Filter.filter
{
    public class WeixiaoAsyncResultFilter : IAsyncResultFilter
    {

        /// <summary>
        /// 动作结果执行后
        /// </summary>
        /// <param name="context"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            await next.Invoke();
            await OnExecutedAsync(context);
        }

        /// <summary>
        /// 执行后
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task OnExecutedAsync(ResultExecutingContext context)
        {
            Console.WriteLine("IAsyncResultFilter 执行");
            await Task.CompletedTask;
        }
    }
}
