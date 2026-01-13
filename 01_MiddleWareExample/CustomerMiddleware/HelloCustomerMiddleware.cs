using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace _01_MiddleWareExample.CustomerMiddleware
{
    // 可以直接添加 "中间件"类, 会自动生成模板, 通过构造函数传入 RequestDelegate 即下一个中间件
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class HelloCustomerMiddleware
    {
        private readonly RequestDelegate _next;

        public HelloCustomerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            // before logic
            if(httpContext.Request.Query.ContainsKey("firstName") && httpContext.Request.Query.ContainsKey("lastName"))
            {
                Console.WriteLine("firstName = " + httpContext.Request.Query["firstName"]);
                Console.WriteLine("lastName = " + httpContext.Request.Query["lastName"]);

                await httpContext.Response.WriteAsync("firstName = " + httpContext.Request.Query["firstName"]);
                await httpContext.Response.WriteAsync("lastName = " + httpContext.Request.Query["lastName"]);
            }

            await _next(httpContext);
            //return _next(httpContext); 使用 await 方法需要标记为 async, 而async后就不能 return
            // later logic
        }
    }

    // 扩展方法 也是自动生成的
    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class HelloCustomerMiddlewareExtensions
    {
        public static IApplicationBuilder UseHelloCustomerMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<HelloCustomerMiddleware>();
        }
    }
}
