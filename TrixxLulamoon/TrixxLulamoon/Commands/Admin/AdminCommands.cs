using Discord.Commands;
using Discord;
using TrixxLulamoon.Config;
using TrixxLulamoon.Utils;

namespace TrixxLulamoon.Commands.Admin
{
    public class AdminCommands : ModuleBase<SocketCommandContext>
    {
        [Command("!buttons")]
        [Summary("Spawning admin buttons")]
        public async Task SpawnAdminButtons()
        {
            if (Context.Channel.Id == ConfigModel.Instance.ChannelsConfig.AdminChannelId)
            {
                var spawnRolesButton = new ButtonBuilder()
                {
                    Label = "Создать роль",
                    CustomId = Consts.ADMIN_SPAWN_MODAL_ROLE_BUTTON_ID,
                    Style = ButtonStyle.Secondary,
                };

                var sendMessageButton = new ButtonBuilder()
                {
                    Label = "Отправить сообщение",
                    CustomId = Consts.ADMIN_SPAWN_MODAL_MESSAGE_BUTTON_ID,
                    Style = ButtonStyle.Success,
                };

                await ReplyAsync(components: new ComponentBuilder().WithButton(spawnRolesButton).Build());
                await ReplyAsync(components: new ComponentBuilder().WithButton(sendMessageButton).Build());
            }
            else
            {
                await ReplyAsync("Отказываюсь");
            }
        }
    }
}
