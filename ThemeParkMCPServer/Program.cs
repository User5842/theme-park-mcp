using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Net.Http.Headers;
using ThemeParkMCPServer.Clients;

var builder = Host.CreateEmptyApplicationBuilder(settings: null);

builder.Services.AddMcpServer()
    .WithStdioServerTransport()
    .WithToolsFromAssembly();

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.PropertyNameCaseInsensitive = true;
});

builder.Services.AddHttpClient<IThemeParkClient, ThemeParkClient>()
    .ConfigureHttpClient(client =>
    {
        client.BaseAddress = new Uri("https://api.themeparks.wiki/v1/");
    });

var app = builder.Build();

await app.RunAsync();
