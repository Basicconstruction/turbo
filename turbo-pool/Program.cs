using System;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);
var port = 8889; // 默认端口号

if (args.Length>0&& int.TryParse(args[0], out var parsedPort))
{
    port = parsedPort; // 如果第1个参数是有效的端口号，使用该端口号
}

builder.WebHost.UseUrls($"http://localhost:{port}");
var app = builder.Build();
var staticFilesPath = ""; // 手动指定的静态文件路径

if (args.Length > 1)
{
    staticFilesPath = args[1]; // 假设第2个参数是静态文件路径
}
if (string.IsNullOrEmpty(staticFilesPath) || !Directory.Exists(staticFilesPath))
{
    staticFilesPath = Path.Combine(Path.GetDirectoryName(Environment.ProcessPath!)!, "wwwroot");
    Console.WriteLine($"No static file path provided. Using default path: {staticFilesPath}");
}
var contentTypeProvider = new FileExtensionContentTypeProvider
{
    Mappings =
    {
        [".js"] = "application/javascript",
        [".css"] = "text/css",
        [".html"] = "text/html",
        [".woff"] = "application/font-woff",
        [".woff"] = "application/font-woff",
        [".woff2"] = "font/woff2",
        [".ttf"] = "application/x-font-ttf",
        [".otf"] = "font/otf",
        [".eot"] = "application/vnd.ms-fontobject",
    }
};
// // 添加静态文件中间件
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(staticFilesPath),
    ContentTypeProvider = contentTypeProvider
});
Console.WriteLine($"Run as static server: at {staticFilesPath}");
Console.WriteLine($"Port: {port}");
// 添加后备文件的功能
app.Use(async (context, next) =>
{
    if (!File.Exists(staticFilesPath+ context.Request.Path))
    {
        await Console.Out.WriteLineAsync(context.Request.Path);
        var fileProvider = new PhysicalFileProvider(staticFilesPath);
        var fileInfo = fileProvider.GetFileInfo("index.html");

        if (!fileInfo.Exists)
        {
            await next();
            return;
        }

        context.Response.ContentType = "text/html";
        await context.Response.SendFileAsync(fileInfo);
    }
    else
    {
        await Console.Out.WriteLineAsync($"contain {staticFilesPath + context.Request.Path}");
        await next();
    }
});

app.Run();