using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreDemo_Core3.Middleware
{
    public class FirstMiddleware
    {
        private readonly RequestDelegate _next;
        /// <summary>
        /// 管道执行到该中间件时候下一个中间件的RequestDelegate请求委托，如果有其它参数，也同样通过注入的方式获得
        /// </summary>
        /// <param name="next"></param>
        public FirstMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext context)
        {
            Console.WriteLine("FirstMiddleware");
            // await _next.Invoke(context);
             await _next(context);
            await context.Response.WriteAsync("Response1");
        }
    }
    /// <summary>
    /// 拓展
    /// </summary>
    public static class FirstMiddlewareExtensions
    {
        public static IApplicationBuilder UseFisrt(this IApplicationBuilder app)
        {
            return app.UseMiddleware<FirstMiddleware>();
        }
    }
}
