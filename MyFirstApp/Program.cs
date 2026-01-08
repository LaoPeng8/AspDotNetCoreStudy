// C# 9 支持的顶层语法, 会编译后自动加上main方法

var builder = WebApplication.CreateBuilder(args);// 加载配置, 环境, 默认服务
var app = builder.Build();

//app.MapGet("/", () => "Hello World!");// 收到请求 / 时, 响应 hellowrold

app.Run(async (HttpContext httpContext) =>
{
    // 请求头
    Console.WriteLine($"接收到 {httpContext.Request.Method} 请求: {httpContext.Request.Path}");
    if(httpContext.Request.Method == "GET")
    {
        if(!string.IsNullOrEmpty(httpContext.Request.QueryString.Value))
            Console.WriteLine("Request.Query: " + httpContext.Request.QueryString.Value);
    }

    // 响应头
    httpContext.Response.Headers["Token"] = "abcdef";
    httpContext.Response.Headers["Content-Type"] = "text/html";

    // 状态码
    httpContext.Response.StatusCode = 400;

    // 响应体
    // 当使用 异步方法时, 需要同步等待执行时使用 await, 当一个方法中使用了 await 时, 该方法需声明为async异步方法; 与Js一致
    await httpContext.Response.WriteAsync("<h1>Hello</h1>");
    await httpContext.Response.WriteAsync("<h2>World</h2>");
});

app.Run();