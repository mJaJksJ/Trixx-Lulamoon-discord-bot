using Discord.Interactions;
using Discord.WebSocket;

namespace DiscordBot.SlashModules
{
    public class MuteMeModule : InteractionModuleBase<SocketInteractionContext>
    {
        [SlashCommand("mute-me", "Замьютить себя на 30сек - 4ч")]
        public async Task PingAsync()
        {
            var user = (SocketGuildUser)Context.User;

            var minSeconds = 30;
            var maxSeconds = (int)TimeSpan.FromHours(4).TotalSeconds;
            double random = Math.Pow(Random.Shared.NextDouble(), 3);
            var seconds = minSeconds + (int)((maxSeconds - minSeconds) * random);
            var timeout = TimeSpan.FromSeconds(seconds);

            await user.SetTimeOutAsync(timeout);
        }
    }
}
