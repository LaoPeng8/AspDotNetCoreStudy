using _01_MiddleWareExample.CustomerMiddleware;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddTransient<MyCustomerMiddleware>();// 注册中间件服务类 (AddTransient会将MyCustomerMiddleware依赖注入,获取服务)
var app = builder.Build();// 程序构建器对象, 用于配置和创建中间件

/*
// 使用Run方法创建中间件(短路中间件)
app.Run(async (HttpContext context) =>
{
    await context.Response.WriteAsync("Hello");
});

// 要记住, app.Run()方法不会将请求转发到其后的任何后续中间件
app.Run(async (HttpContext context) =>
{
    await context.Response.WriteAsync("Hello Again");
});
*/

// 顺序不能乱, 第一个就是第一个被调用的, 第二个就是第二个
// 使用Use方法创建中间件
app.Use(async (HttpContext context, RequestDelegate next) =>
{
    await context.Response.WriteAsync("Hello");
    await next(context);// 必须将 context 作为参数传递给下一个中间件
});

app.UseMyCustomMiddleware();// 使用扩展方法代替直接使用 UseMiddleware<MyCustomerMiddleware>();
//app.UseMiddleware<MyCustomerMiddleware>();// 自定义中间件
//app.Use(async (HttpContext context, RequestDelegate next) =>
//{
//    await context.Response.WriteAsync("Hello Again");
//    await next(context);// 继续调用下一个中间件 (可以选择性的调用, 不调用就不会执行后续中间件, 可以提前响应)
//});

app.UseHelloCustomerMiddleware();

// 当 context.Request.Query.ContainsKey("Hello") 为true时
// 才会执行第二个Lambda即 使用Use方法创建的中间件
app.UseWhen(
    (context) =>
    {
        return context.Request.Query.ContainsKey("Hello");
    },
    (app) =>
    {
        app.Use(async (context, next) =>
        {
            await context.Response.WriteAsync("Hello from Middleware branch");
            await next();
        });
    }
);

app.Run(async (HttpContext context) =>
{
    await context.Response.WriteAsync("Hello Again too");
});

app.Run();
