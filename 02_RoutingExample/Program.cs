using Microsoft.AspNetCore.Http;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// 处理中文乱码中间件
app.Use(async (context, next) =>
{
    context.Response.ContentType = "text/html; charset=utf-8";
    await next(context);
});

app.Use(async (context, next) =>
{
    
    Microsoft.AspNetCore.Http.Endpoint endpoint = context.GetEndpoint();// 在 app.UseRouting(); 前获取 endpoint 是获取不到的为 null
    if(endpoint == null)
    {
        Console.WriteLine("Microsoft.AspNetCore.Http.Endpoint is null");
    }
    await next(context);// 继续下一个中间件
});

// 启用路由
app.UseRouting();

app.Use(async (context, next) =>
{
    // 在 app.UseRouting(); 之后获取 endpoint 是可以获取到的
    // 但不一定 100% 可以获取到, 只有当识别到路由后才可以获取到, 例如请求 /map1 /map2 是可以获取到的, 请求此处没有的路由 /map12345 就获取不到
    Microsoft.AspNetCore.Http.Endpoint endpoint = context.GetEndpoint();
    if (endpoint != null)
    {
        await context.Response.WriteAsync("Endpoint.DisplayName: " + endpoint.DisplayName + "\n" + "<br/>");
    }
    await next(context);// 继续下一个中间件
    // 当以 get方式请求 map2时会报错 System.InvalidOperationException:“Headers are read-only, response has already started.”
    // 原因似乎是 app.UseRouting(); 路由匹配后, app.UseEndpoints()端点执行前 写入了响应, 执行next时会进入实际端点,
    // 端点处发现是get请求, 但map2是 MapPost() 所以立即响应 405 方法不被允许, 然后回到 next()执行后(中间件如何一层一层进入的,此时如何一层一层退出, 直至最终响应)
    // 响应为 405 方法不被允许, 但 response钟在此处写入了内容, 所以报错
});

// 创建 endpoint
app.UseEndpoints(endpoints =>
{
    // 添加 endpoint
    // 均以 map* 开头, 例如 endpoints.mapGet() .mapPost() .mapControllers()
    endpoints.Map("map1", async (context) => {
        await context.Response.WriteAsync("In Map 1");// 访问 /map1 会响应 (get post 均可)
    });

    endpoints.MapPost("map2", async (context) => {
        await context.Response.WriteAsync("In Map 2");// 访问 /map2 会响应 (仅 post 响应, 非post请求后不响应, 并且虽然不响应但好像还是被此路由拦截了, 并未执行下方的 app.Run())
        // 并非不响应, 而且响应了状态码 405 Method Not Allowed
    });

    // 当请求 /files/a 或 files/nginx. 或 files/nginxtxt 等 匹配不上路由字面量时, 则请求不会到达该接口, 在此处会由短路中间件Run捕获到
    endpoints.Map("files/{fileName}.{extension}", async (HttpContext context) =>
    {
        object fileNameObj = context.Request.RouteValues["fileName"];
        object extensionObj = context.Request.RouteValues["extension"];

        string fileName = Convert.ToString(fileNameObj);
        string extension = Convert.ToString(extensionObj);

        await context.Response.WriteAsync($"你请求了文件: {fileName}.{extension}");
    });

    // 可以为路由值通过类似给方法参数设置默认值的方式, 来给路由参数设置默认值, 当没有输入该参数时会有一个默认值此处为harsha
    endpoints.Map("employee/profile/{EmployeeName=harsha}", async (HttpContext context) =>
    {
        string employeeName = Convert.ToString(context.Request.RouteValues["employeename"]);// 获取路由值时不区分大小写

        await context.Response.WriteAsync($"In Employee Profile - {employeeName}");
    });

    // 路由参数默认值的实际用途
    endpoints.Map("/products/details/{id=1}", async context =>
    {
        int id = Convert.ToInt32(context.Request.RouteValues["id"]);
        await context.Response.WriteAsync($"Products details - {id}");
    });

    // 路由参数可以为空
    endpoints.Map("/saleOrder/info/{id?}", async context =>
    {
        if (!context.Request.RouteValues.ContainsKey("id"))
        {
            await context.Response.WriteAsync("缺少参数 id");
            return;
        }

        int id = Convert.ToInt32(context.Request.RouteValues["id"]);
        await context.Response.WriteAsync($"saleOrder info - {id}");
    });
});

app.Run(async (context) =>
{
    await context.Response.WriteAsync("这是短路中间件 .Run() 当没有任何路由被匹配时会执行(get post 均可), 且我不会将请求转发到下一个中间件<br/>");
    await context.Response.WriteAsync("我接收到的请求是: " + context.Request.Path);
});

app.Run();
