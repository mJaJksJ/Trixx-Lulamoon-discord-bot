using Discord.Interactions;
using Discord.WebSocket;
using TrixxLulamoon.Utils;

namespace TrixxLulamoon.Interactions.Users
{
    public class UsersButtonsHandler
    {
        private readonly DiscordShardedClient _discordShardedClient;
        private readonly DiscordSocketClient _discordSocketClient;
        private readonly Serilog.ILogger _logger;

        public UsersButtonsHandler(
            DiscordShardedClient discordShardedClient,
            DiscordSocketClient discordSocketClient,
            Serilog.ILogger logger
        )
        {
            _discordShardedClient = discordShardedClient;
            _discordSocketClient = discordSocketClient;
            _logger = logger;
        }

        public async Task ButtonExecuted(SocketMessageComponent component)
        {
            var customId = component.Data.CustomId;
            var context = new ShardedInteractionContext(_discordShardedClient, component);
            var user = _discordShardedClient.Guilds.First().GetUser(component.User.Id);

            if (customId.StartsWith(Consts.YES_ROLE_BUTTON_ID))
            {
                await ChangeRole(component, customId, true, user);
            }

            if (customId.StartsWith(Consts.NO_ROLE_BUTTON_ID))
            {
                await ChangeRole(component, customId, false, user);
            }
        }

        public async Task ChangeRole(SocketMessageComponent component, string customId, bool isYes, SocketGuildUser user)
        {
            var role = customId.Replace(isYes ? Consts.YES_ROLE_BUTTON_ID : Consts.NO_ROLE_BUTTON_ID, string.Empty);
            if (ulong.TryParse(role, out var roleId) && roleId != 0)
            {
                if (isYes)
                {
                    await user.AddRoleAsync(roleId);
                }
                else
                {
                    await user.RemoveRoleAsync(roleId);
                }
                await component.RespondAsync($"Вы {(isYes ? "получили" : "покинули")} роль {MentionHelper.MentionRole(role)}", ephemeral: true);
            }
        }
    }
}
