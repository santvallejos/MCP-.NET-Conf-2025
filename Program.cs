using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

Console.WriteLine("Iniciando tools");

var builder = Host.CreateEmptyApplicationBuilder(settings: null);

// Create the MCP Server with Standard I/O Transport and Tools from the current assembly
builder.Services.AddMcpServer()
    .WithStdioServerTransport()
    .WithToolsFromAssembly();

builder.Logging.AddConsole(options =>
{
    options.LogToStandardErrorThreshold = LogLevel.Trace;
});

builder.Services.AddHttpClient();

var app = builder.Build();

await app.RunAsync();