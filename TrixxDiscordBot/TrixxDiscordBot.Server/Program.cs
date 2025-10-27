using Microsoft.AspNetCore.HttpOverrides;
using Serilog;
using System.Text.Json.Serialization;
using Trixx.Cartoons.Database;
using Trixx.Common;
using Trixx.Core;
using Trixx.Database;
using TrixxDiscordBot.Server.Startup.Auth;
using TrixxDiscordBot.Server.Startup.Swagger;
using System.Text.Json.Serialization;
using Trixx.Cartoons;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

const string SERILOG_OUTPUT_TEMPLATE = "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] <{ThreadId}> :: {Message:lj}{NewLine}{Exception}";
builder.Host.UseSerilog((context, services, configuration) => configuration
    .ReadFrom.Configuration(context.Configuration)
    .ReadFrom.Services(services)
    .Enrich.FromLogContext()
    .Enrich.WithThreadId()
    .WriteTo.File(
        outputTemplate: SERILOG_OUTPUT_TEMPLATE,
        path: Path.Combine("/logs/trixx_lulamoon", "log_trixx"),
        shared: true,
        rollingInterval: RollingInterval.Day,
        fileSizeLimitBytes: 128 * 1024 * 1024
    )
    .WriteTo.Console(outputTemplate: SERILOG_OUTPUT_TEMPLATE)
);

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);

builder.Services
    .AddCommon()
    .AddTrixxDatabases(configuration)
    .AddTrixxCartoonsDatabases(configuration)
    .AddTrixxSwaggerGen()
    .AddTrixxIdentity(configuration)
    .AddTrixxCartoons()
    .AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumMemberConverter());
    });

builder.Services
    .AddTrixxCore();

var app = builder.Build();
var env = app.Environment;

if (env.IsDevelopment())
{
    app.UseTrixxSwagger();
}

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor |
        ForwardedHeaders.XForwardedProto
});

app.UseRouting();
app.UseTrixxJwt();
app.MapControllers();

app.Services.MigrateTrixxDatabase();
app.Services.MigrateTrixxCartoonsDatabase();

app.UseDefaultFiles();
app.UseStaticFiles();

app.Run();
