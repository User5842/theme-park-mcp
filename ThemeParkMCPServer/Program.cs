using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ModelContextProtocol;
using ModelContextProtocol.AspNetCore;
using System.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMcpServer()
    .WithHttpTransport()
    .WithToolsFromAssembly();

builder.Services.AddSingleton(_ =>
{
    var client = new HttpClient() { BaseAddress = new Uri("https://api.themeparks.wiki/v1/") };
    client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("themepark-tool", "1.0"));
    return client;
});

var app = builder.Build();

app.MapMcp();

app.Run();
