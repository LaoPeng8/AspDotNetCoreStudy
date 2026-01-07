// C# 9 支持的顶层语法, 会编译后自动加上main方法

var builder = WebApplication.CreateBuilder(args);// 加载配置, 环境, 默认服务 (builder.configuration访问配置, builder.Services访问服务, builder.Environment访问环境变量)
var app = builder.Build();

app.MapGet("/", () => "Hello World!");// 收到请求 / 时, 响应 hellowrold

app.Run();// 启动