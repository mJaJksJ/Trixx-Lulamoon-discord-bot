using Discord.Commands;
using Discord.Interactions;
using Discord.WebSocket;
using DiscordBot;
using DiscordBot.Middlewares;
using DiscordBot.Utils;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var discordConfig = new DiscordSocketConfig()
{
    ResponseInternalTimeCheck = !builder.Environment.IsDevelopment(),
};

builder.Services
    .AddSingleton(discordConfig)
    .AddSingleton<DiscordSocketClient>(sp =>
    {
        var config = sp.GetRequiredService<DiscordSocketConfig>();
        return new DiscordSocketClient(config);
    })
    .AddSingleton<InteractionService>(sp =>
    {
        var client = sp.GetRequiredService<DiscordSocketClient>();
        return new InteractionService(client);
    });

builder.Services
    .AddSingleton<CommandService>()
    .AddSingleton<CommandHandler>();

builder.Services.AddControllers();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.MapControllers();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

await app.Services.GetRequiredService<CommandHandler>().InstallCommandsAsync();
var socketClient = app.Services.GetRequiredService<DiscordSocketClient>();
await DiscordClientUtils.StartSocketAsync(socketClient);

app.Run();
