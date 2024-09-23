using Discord.Commands;
using Discord.WebSocket;
using Serilog;
using TrixxLulamoon;
using TrixxLulamoon.Buttons;
using TrixxLulamoon.Config;
using TrixxLulamoon.Utils;

var builder = WebApplication.CreateBuilder(args);

ConfigModel.Create(builder.Configuration);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region log config

builder.Host.UseSerilog((context, services, configuration) => configuration
    .ReadFrom.Configuration(context.Configuration)
    .ReadFrom.Services(services)
    .Enrich.FromLogContext()
    .Enrich.WithThreadId()
    .WriteTo.File(
        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] <{ThreadId}> :: {Message:lj}{NewLine}{Exception}",
        path: Path.Combine("/logs/trixx_lulamoon", "log_trixx"),
        shared: true,
        rollingInterval: RollingInterval.Day,
        fileSizeLimitBytes: 128 * 1024 * 1024
    )
);

#endregion log config

#region DiscordSocketClient
var discordConfig = new DiscordSocketConfig();
builder.Services
    .AddSingleton<DiscordSocketClient>()
    .AddSingleton<CommandService>()
    .AddSingleton<CommandHandler>()
    .AddSingleton<ButtonsHandler>();
#endregion DiscordSocketClient

var app = builder.Build();

app.UseSerilogRequestLogging();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

await app.Services.GetRequiredService<CommandHandler>().InstallCommandsAsync();

var client = app.Services.GetRequiredService<DiscordSocketClient>();
await DiscordClientUtils.StartAsync(client);
client.ButtonExecuted += new ButtonsHandler(client, app.Services.GetRequiredService<Serilog.ILogger>()).MyButtonHandler;


client.Log += DiscordClientUtils.LogAsync;

app.Run();

