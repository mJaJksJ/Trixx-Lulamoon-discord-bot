using Discord;
using Discord.Commands;
using Discord.Interactions;
using Discord.WebSocket;
using DiscordBot;
using DiscordBot.Middlewares;
using DiscordBot.MongoDb;
using DiscordBot.Utils;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);
IConfiguration configuration = builder.Configuration;

builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection("MongoDbSettings"));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var discordConfig = new DiscordSocketConfig()
{
    ResponseInternalTimeCheck = !builder.Environment.IsDevelopment(),
    GatewayIntents =
        GatewayIntents.Guilds |
        GatewayIntents.GuildMessages |
        GatewayIntents.GuildMembers |
        GatewayIntents.MessageContent |
        GatewayIntents.GuildScheduledEvents,
};

builder.Services
    .AddSingleton<IMongoClient>(sp =>
    {
        var settings = sp.GetRequiredService<IOptions<MongoDbSettings>>().Value;
        return new MongoClient(settings.ConnectionString);
    })
    .AddSingleton<MongoDbContext>();

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
    .AddSingleton<CommandHandler>()
    .AddSingleton<InteractionHandler>();

builder.Services.AddControllers();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.MapControllers();

app.UseSwagger(c =>
{
    c.RouteTemplate = "swagger-discordbot/v1/swagger.json";
});
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger-discordbot/v1/swagger.json", "API V1");
    c.RoutePrefix = "swagger-discordbot";
});

await app.Services.GetRequiredService<CommandHandler>().InstallCommandsAsync();
await app.Services.GetRequiredService<InteractionHandler>().InstallInteractionsAsync();
var socketClient = app.Services.GetRequiredService<DiscordSocketClient>();
await DiscordClientUtils.StartSocketAsync(configuration, socketClient);

app.Run();
