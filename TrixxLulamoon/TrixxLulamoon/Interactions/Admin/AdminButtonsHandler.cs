using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using TrixxLulamoon.Config;
using TrixxLulamoon.Utils;

namespace TrixxLulamoon.Interactions.Admin
{
    public class AdminButtonsHandler
    {
        private readonly DiscordShardedClient _discordShardedClient;
        private readonly Serilog.ILogger _logger;

        public AdminButtonsHandler(
            DiscordShardedClient discordShardedClient,
            Serilog.ILogger logger
        )
        {
            _discordShardedClient = discordShardedClient;
            _logger = logger;
        }

        public async Task ButtonExecuted(SocketMessageComponent component)
        {
            var customId = component.Data.CustomId;
            var context = new ShardedInteractionContext(_discordShardedClient, component);
            switch (customId)
            {
                case Consts.ADMIN_SPAWN_MODAL_ROLE_BUTTON_ID:
                    await SendSpawnRoleButtons(context);
                    break;

                case Consts.ADMIN_SPAWN_MODAL_MESSAGE_BUTTON_ID:
                    await SpawnSendMessageModal(context);
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

        public async Task SendSpawnRoleButtons(ShardedInteractionContext context)
        {
            var mb = new ModalBuilder()
                .WithTitle("Создать кнопки смены роли")
                .WithCustomId(Consts.ROLE_BUTTON_MODAL_ID)
                .AddTextInput("Канал", Consts.ROLE_BUTTON_MODAL_CHANNEL_ID, required: true)
                .AddTextInput("Id роли", Consts.ROLE_BUTTON_MODAL_ROLE_ID, required: true);

            await context.Interaction.RespondWithModalAsync(mb.Build());
        }

        public async Task SpawnSendMessageModal(ShardedInteractionContext context)
        {
            var mb = new ModalBuilder()
                .WithTitle("Отправить сообщение")
                .WithCustomId(Consts.MESSAGE_MODAL_ID)
                .AddTextInput("Канал", Consts.MESSAGE_MODAL_CHANNEL_ID, required: true, value: ConfigModel.Instance.ChannelsConfig.DefaultChatChannelId.ToString())
                .AddTextInput("Ответ на (необязательно)", Consts.MESSAGE_MODAL_RESPONSABLE_MESSAGE_ID, required: false)
                .AddTextInput("Сообщение", Consts.MESSAGE_MODAL_TEXT_ID, required: true, style: TextInputStyle.Paragraph);

            await context.Interaction.RespondWithModalAsync(mb.Build());
        }
    }
}
