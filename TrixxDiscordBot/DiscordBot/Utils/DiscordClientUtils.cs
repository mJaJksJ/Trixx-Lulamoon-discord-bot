using Discord.WebSocket;
using Discord;

namespace DiscordBot.Utils
{
    public class DiscordClientUtils
    {
        public async static Task StartSocketAsync(params BaseSocketClient[] clients)
        {
            foreach (var client in clients)
            {
                await client.LoginAsync(TokenType.Bot, "TODO: token");
                await client.StartAsync();
            }
        }
    }
}
