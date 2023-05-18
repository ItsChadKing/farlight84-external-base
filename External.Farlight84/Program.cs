using External.Farlight84.Events;
using External.Farlight84.Services;
using External.Farlight84;
using External.Farlight84.Drawing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = GetHostBuilder(args).Build();
var loaderService = host.Services.GetRequiredService<GameHandler>();

await loaderService.Initialize();

await host.RunAsync();

static IHostBuilder GetHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
        .ConfigureServices((context, services) =>
            services
                .AddHostedService<WorldRetrievalService>()
                .AddHostedService<EspRetrievalService>()
                .AddTransient<GameHandler>()
                .AddTransient<IGameEvent, GameEvent>()
                .AddTransient<IGameWindowDrawing, GameWindowDrawing>());