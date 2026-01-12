
namespace _01_MiddleWareExample.CustomerMiddleware
{
    /// <summary>
    /// 自定义中间件
    ///     当中间件逻辑较多时, 不方便使用 app.Run() app.Use(), 实现 IMiddleware 将逻辑抽离为单独一个类来实现
    /// </summary>
    public class MyCustomerMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            await context.Response.WriteAsync("My Customer Middleware - Starts");
            await next(context);
            await context.Response.WriteAsync("My Customer Middleware - Ends");
        }
    }


    /// <summary>
    /// 扩展方法
    /// 因为 var builder = WebApplication.CreateBuilder(args); 返回的 WebApplicationBuilder 对象
    /// </summary>
    public static class CustomerMiddlewareExtension
    {
        // 命名遵循规范 以 Use 开头 Middleware表示中间件
        public static IApplicationBuilder UseMyCustomMiddleware(this IApplicationBuilder app)
        {
            return app.UseMiddleware<MyCustomerMiddleware>();
        }
    }
}
