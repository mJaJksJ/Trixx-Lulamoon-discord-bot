using Discord.WebSocket;
using System.Runtime.InteropServices.JavaScript;
using TrixxLulamoon.Config;
using TrixxLulamoon.Utils;

namespace TrixxLulamoon.Buttons
{
    public class ButtonsHandler
    {
        private readonly DiscordSocketClient _discordSocketClient;
        private readonly Serilog.ILogger _logger;

        public ButtonsHandler(
            DiscordSocketClient discordSocketClient,
            Serilog.ILogger logger
        )
        {
            _discordSocketClient = discordSocketClient;
            _logger = logger;
        }

        public async Task MyButtonHandler(SocketMessageComponent component)
        {
            var user = _discordSocketClient.Guilds.First().GetUser(component.User.Id);

            ButtonIdType? customId = null;
            try
            {
                customId = (ButtonIdType)int.Parse(component.Data.CustomId);
            }
            catch (Exception ex)
            {
                _logger.Warning(ex, $"try parse {component.Data.CustomId}");
            }

            var isAdd = false;
            var role = ulong.Parse(ConfigModel.Instance.RolesConfig.GetRoleId(customId.Value));
            switch (customId)
            {
                case ButtonIdType.Fluttershy:
                case ButtonIdType.DellArte:
                case ButtonIdType.Zetsu:
                case ButtonIdType.Mythical:
                case ButtonIdType.Musical:
                    isAdd = true;                    
                    break;
                case ButtonIdType.NotFluttershy:
                case ButtonIdType.NotDellArte:
                case ButtonIdType.NotZetsu:
                case ButtonIdType.NotMythical:
                case ButtonIdType.NotMusical:
                    isAdd = false;
                    break;
            }

            if (isAdd)
            {
                await user.AddRoleAsync(role);
            }
            else
            {
                await user.RemoveRoleAsync(role);
            }

            _logger.Information($"Пользователь {user.Nickname} ({user.DisplayName}) {(isAdd ? "получил" : "покинул")} роль <@&{role}>");
            await component.RespondAsync($"Вы {(isAdd ? "получили" : "покинули")} роль <@&{role}>", ephemeral: true);
        }
    }
}
