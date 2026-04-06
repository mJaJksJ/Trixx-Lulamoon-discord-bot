using Discord;
using Discord.Commands;
using Discord.WebSocket;
using DiscordBot.Utils;
using System.Reflection;
using System.Text.RegularExpressions;

namespace DiscordBot
{
    public class CommandHandler
    {
        private readonly DiscordSocketClient _client;
        private readonly CommandService _commands;
        private readonly IServiceProvider _services;

        public CommandHandler(
            DiscordSocketClient client,
            CommandService commands,
            IServiceProvider services)
        {
            _commands = commands;
            _client = client;
            _services = services;
        }

        public async Task InstallCommandsAsync()
        {
            _client.MessageReceived += HandleCommandAsync;

            await _commands.AddModulesAsync(
                assembly: Assembly.GetEntryAssembly(),
                services: _services);
        }

        private async Task HandleCommandAsync(SocketMessage messageParam)
        {
            var message = messageParam as SocketUserMessage;
            if (message == null) return;

            await AnswerIfEmptyTag(message);

            int argPos = 0;
            if (!(message.HasCharPrefix('!', ref argPos) ||
                message.HasMentionPrefix(_client.CurrentUser, ref argPos)) ||
                message.Author.IsBot)
            {
                return;
            }

            var context = new SocketCommandContext(_client, message);

            await _commands.ExecuteAsync(
                context: context,
                argPos: argPos,
                services: null);
        }

        private async Task AnswerIfEmptyTag(SocketUserMessage message)
        {
            var mention = MentionHelper.MentionUser(_client.CurrentUser.Id.ToString());
            if (!message.Content.Contains(mention))
            {
                return;
            }
            var withoutMention = message.Content.Replace(mention, string.Empty);
            if (Regex.Replace(withoutMention, @"\s+", string.Empty).Length == 0)
            {
                await message.ReplyAsync("Я вернулся Ниндзяго!");
            }
        }
    }
}
