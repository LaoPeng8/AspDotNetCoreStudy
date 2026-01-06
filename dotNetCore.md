# ASP.Net Core 9



## 介绍

1. 跨平台
2. 可以创建任何规模的Web应用, 包括小型/中型和复杂应用, 以及创建Rest API服务
3. Asp .NET Core 默认使用Kestrel作为应用服务器, 其他反向代理包括 IIS, Nginx, Docker
4. 开源的 `https://github.com/dotnet/aspnetcore`
5. 开箱即支持 Microsoft Azure云服务
6. ASP.NET Core 包含四个部分
   * Asp.Net Core Mvc 大多数Web应用都是它开发的, 但这不应该与早期的 Asp.Net MVC混淆, 当你在Asp.NET Core 中使用模型, 视图, 控制器 模式时, 就称为 Asp.Net Core MVC
   * Asp.Net Core Web API 当你只创建包含模型的控制器但是没有视图, 则称为Asp.NET Core Web API, 通常 Asp.NET Core Web API 用于创建RESTful服务, 接收请求并以数据形式返回响应, 搭配单独的前端如 Vue, React
   * Asp.Net Core Razor Pages 服务于简单且以页面为中心的场景
   * Asp.Net Core Blazor 如果开发团队希望在服务器端与客户端都仅使用C#语言, 则更倾向于使用Asp.NET Core Blazor

## 比较

Asp.Net Web Forms    2002

* 主要存在性能劣势, 由于服务器状态和视图状态, 它的性能较慢, 因为Asp.Net WebForms 本身试图使Web变为有状态而非无状态, 这意味着每个页面都需要存储状态, 对于大型项目来说这非常消耗资源
* 不开源

Asp.Net Mvc    2009

* 是建立在早期为Asp.Net WebForms 开发的一些组件上的. 例如`system.web.dll`它在某些方法提供了部分较慢的性能并且缺乏对跨平台的支持, 与.NET框架的某些组件紧密耦合, 使得在其他操作系统上托管变得异常困难
* 开源 但 .Net Framework 不开源 (Asp.Net Mvc 基于 .Net Framework工作)
* 选择性添加依赖注入

Asp.Net Core    2016

* 跨平台, 因为Asp.Net Core 基于.Net Core工作, 它也是跨平台的,  支持云服务
* 开源 且 .Net Core 开源
* 内置依赖注入