using Discord.Commands;
using Discord.Interactions;
using Discord.WebSocket;
using Serilog;
using TrixxLulamoon;
using TrixxLulamoon.Config;
using TrixxLulamoon.Interactions.Admin;
using TrixxLulamoon.Interactions.Users;
using TrixxLulamoon.Utils;

var builder = WebApplication.CreateBuilder(args);
IConfiguration configuration = builder.Configuration;

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

#region DiscordClient
var discordConfig = new DiscordSocketConfig()
{
    ResponseInternalTimeCheck = false
};

builder.Services
    .AddSingleton(new InteractionService(new Discord.Rest.DiscordRestClient(discordConfig)))
    .AddSingleton(new DiscordSocketClient(discordConfig))
    .AddSingleton(new DiscordShardedClient(discordConfig));

builder.Services
    .AddSingleton<CommandService>()
    .AddSingleton<CommandHandler>();

builder.Services
    .AddSingleton<AdminButtonsHandler>()
    .AddSingleton<AdminModalsHandler>()
    .AddSingleton<UsersButtonsHandler>()
    .AddSingleton<InteractionHandler>();

#endregion DiscordClient

var app = builder.Build();

app.UseSerilogRequestLogging();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

await app.Services.GetRequiredService<CommandHandler>().InstallCommandsAsync();
await app.Services.GetRequiredService<InteractionHandler>().InstallCommandsAsync();

var socketClient = app.Services.GetRequiredService<DiscordSocketClient>();
var shardedClient = app.Services.GetRequiredService<DiscordShardedClient>();

await DiscordClientUtils.StartSocketAsync(socketClient, shardedClient);

socketClient.Log += DiscordClientUtils.LogAsync;
shardedClient.Log += DiscordClientUtils.LogAsync;

app.Run();
