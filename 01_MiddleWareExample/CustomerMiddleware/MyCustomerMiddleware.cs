
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
    /// 
    /// 在Program.cs中直接调用 app.UseMiddleware<MyCustomerMiddleware>(); 不太优雅
    /// 一般会通过扩展方法来使用, 例直接调用 app.UseMyCustomMiddleware();// 使用扩展方法代替直接使用 UseMiddleware<MyCustomerMiddleware>();
    /// 这是更正式和快捷的方式调用自定义中间件, 在实际项目中,创建扩展方法与创建自定义中间件一起是一种惯例和通用做法
    /// 
    /// 因为 var app = builder.Build(); 返回的 WebApplication 对象
    /// WebApplication 实现至 IApplicationBuilder 接口, 所以我们一般给 IApplicationBuilder 类型注册扩容方法
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
