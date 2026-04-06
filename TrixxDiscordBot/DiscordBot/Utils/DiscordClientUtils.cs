using Discord.WebSocket;
using Discord;

namespace DiscordBot.Utils
{
    public class DiscordClientUtils
    {
        public async static Task StartSocketAsync(IConfiguration configuration, BaseSocketClient client)
        {
            var token = configuration.GetValue<string>("DicordBotToken_InitValue");
            await client.LoginAsync(TokenType.Bot, token);
            await client.StartAsync();
        }
    }
}
