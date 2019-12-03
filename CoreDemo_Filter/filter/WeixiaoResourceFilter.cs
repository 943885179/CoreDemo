using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreDemo_Filter.filter
{
    public class WeixiaoResourceFilter : IResourceFilter
    {
        /// <summary>
        /// 获取资源后拦截
        /// </summary>
        /// <param name="context"></param>
        public void OnResourceExecuted(ResourceExecutedContext context)
        {
            Console.WriteLine("资源拦截执行后");
        }
        /// <summary>
        /// 获取资源前拦截
        /// </summary>
        /// <param name="context"></param>
        public void OnResourceExecuting(ResourceExecutingContext context)
        {

            Console.WriteLine("资源拦截执行前");
        }
    }
}
