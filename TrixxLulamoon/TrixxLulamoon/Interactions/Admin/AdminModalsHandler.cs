using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using TrixxLulamoon.Utils;

namespace TrixxLulamoon.Interactions.Admin
{
    public class AdminModalsHandler
    {
        private readonly DiscordShardedClient _discordShardedClient;
        private readonly DiscordSocketClient _discordSocketClient;
        private readonly Serilog.ILogger _logger;

        public AdminModalsHandler(
            DiscordShardedClient discordShardedClient,
            DiscordSocketClient discordSocketClient,
            Serilog.ILogger logger
        )
        {
            _discordShardedClient = discordShardedClient;
            _discordSocketClient = discordSocketClient;
            _logger = logger;
        }

        public async Task ModalSubmitted(SocketModal socketModal)
        {
            var customId = socketModal.Data.CustomId;
            var context = new ShardedInteractionContext(_discordShardedClient, socketModal);

            switch (customId)
            {
                case Consts.ROLE_BUTTON_MODAL_ID:
                    await SendRoleButtons(socketModal);
                    break;

                case Consts.MESSAGE_MODAL_ID:
                    await SendMessage(socketModal);
                    break;

                default:
                    await ErrorMessage(context);
                    break;
            }
        }

        public async Task ErrorMessage(ShardedInteractionContext context)
        {
            await context.Interaction.RespondAsync("...Button clicked error...");
        }

        public async Task SendRoleButtons(SocketModal socketModal)
        {
            var data = socketModal.Data as IModalInteractionData;
            var channel = data.Components.FirstOrDefault(x => x.CustomId == Consts.ROLE_BUTTON_MODAL_CHANNEL_ID).Value;
            var roleId = data.Components.FirstOrDefault(x => x.CustomId == Consts.ROLE_BUTTON_MODAL_ROLE_ID).Value;
            
            var yesButton = new ButtonBuilder()
            {
                Label = "Хочу",
                CustomId = Consts.YesRoleButton(roleId),
                Style = ButtonStyle.Success,
            };

            var noButton = new ButtonBuilder()
            {
                Label = "Больше нет",
                CustomId = Consts.NoRoleButton(roleId),
                Style = ButtonStyle.Danger,
            };

            if (ulong.TryParse(channel, out var channelValue) && channelValue != 0)
            {
                var socket = _discordSocketClient.GetChannel(channelValue) as IMessageChannel;
                await socket.SendMessageAsync($"Хотите ли вы иметь роль {MentionHelper.MentionRole(roleId)}?");
                await socket.SendMessageAsync("", components: new ComponentBuilder().WithButton(yesButton).Build());
                await socket.SendMessageAsync("", components: new ComponentBuilder().WithButton(noButton).Build());
            }
        }

        public async Task SendMessage(SocketModal socketModal)
        {
            var data = socketModal.Data as IModalInteractionData;
            var channel = data.Components.FirstOrDefault(x => x.CustomId == Consts.MESSAGE_MODAL_CHANNEL_ID).Value;
            var responsable = data.Components.FirstOrDefault(x => x.CustomId == Consts.MESSAGE_MODAL_RESPONSABLE_MESSAGE_ID).Value;
            var message = data.Components.FirstOrDefault(x => x.CustomId == Consts.MESSAGE_MODAL_TEXT_ID).Value;

            if (ulong.TryParse(channel, out var channelValue) && channelValue != 0)
            {
                var socket = _discordSocketClient.GetChannel(channelValue) as IMessageChannel;
                MessageReference messageReference = null;
                if (ulong.TryParse(responsable, out var responsableValue) && responsableValue != 0)
                {
                    messageReference = new MessageReference(messageId: responsableValue);
                }
                await socket.SendMessageAsync(text: message, messageReference: messageReference);
            }
        }
    }
}
