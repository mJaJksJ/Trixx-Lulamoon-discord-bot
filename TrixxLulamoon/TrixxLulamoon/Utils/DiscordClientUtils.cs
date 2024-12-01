using Discord;
using Discord.WebSocket;
using Serilog.Events;
using TrixxLulamoon.Config;

namespace TrixxLulamoon.Utils
{
    public static class DiscordClientUtils
    {
        public async static Task StartSocketAsync(params BaseSocketClient[] clients)
        {
            foreach (var client in clients) 
            {
                await client.LoginAsync(TokenType.Bot, ConfigModel.Instance.DiscordConfig.Token);
                await client.StartAsync();
            }
        }

        public async static Task LogAsync(LogMessage message)
        {
            Serilog.Log.Write(DiscordToSerilogLevel(message.Severity), message.Exception, message.Message);
            await Task.CompletedTask;
        }

        private static LogEventLevel DiscordToSerilogLevel(LogSeverity severity)
        {
            return severity switch
            {
                LogSeverity.Error => LogEventLevel.Error,
                LogSeverity.Warning => LogEventLevel.Warning,
                LogSeverity.Critical => LogEventLevel.Fatal,
                LogSeverity.Info => LogEventLevel.Information,
                LogSeverity.Verbose => LogEventLevel.Verbose,
                LogSeverity.Debug => LogEventLevel.Debug,
                _ => throw new NotImplementedException()
            };
        }
    }
}
