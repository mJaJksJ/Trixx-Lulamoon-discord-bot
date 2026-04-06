using Discord.WebSocket;
using Discord;
using Microsoft.AspNetCore.Mvc;
using DiscordBot.Exceptions;

namespace DiscordBot.Api.Controllers.Messages
{
    public class MessagesController(DiscordSocketClient client) : BaseController
    {
        private readonly DiscordSocketClient _client = client;

        [HttpPost("send-message")]
        public async Task SendMessage(
            ulong channelId,
            string message)
        {
            if (_client.GetChannel(channelId) is not IMessageChannel channel)
            {
                throw new DiscordBotNotFoundException($"Канал {channelId} не найден");
            }

            await channel.SendMessageAsync(message);
        }
    }
}
