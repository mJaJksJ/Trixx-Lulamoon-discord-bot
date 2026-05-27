using Discord;
using Discord.WebSocket;
using DiscordBot.MongoDb;
using MongoDB.Driver;

namespace DiscordBot.Utils.InteractionUtils
{
    public static class ScheduleEventsInteractionHelper
    {
        public static async Task ScheduleEventUpdatedAsync(Cacheable<SocketGuildEvent, ulong> before, SocketGuildEvent after, DiscordSocketClient client, MongoDbContext mongoDbContext)
        {
            if (after.Channel?.Id is null)
            {
                return;
            }

            var eventChannel = client.GetChannel(after.Channel.Id);

            if (eventChannel is not IVoiceChannel voiceChannel)
            {
                return;
            }

            var textChannelId = await mongoDbContext.VoiceToTextChannels
                .Find(x => x.VoiceChannelId == voiceChannel.Id)
                .Project(x => x.TextChannelId)
                .FirstOrDefaultAsync();

            var messageChannel = client.GetChannel(textChannelId);

            if (messageChannel is not ITextChannel textChannel)
            {
                return;
            }

            if (!before.HasValue)
            {
                return;
            }

            if (before.Value.StartTime != after.StartTime)
            {
                var eventsUsers = await after.GetUsersAsync(100, null);
                await textChannel.SendMessageAsync($@"
⚠️ Событие **{after.Name}** обновлено. Время начала: ~~{before.Value.StartTime:dd.MM.yyyy HH:mm}~~ -> **{after.StartTime:dd.MM.yyyy HH:mm}** ⚠️
{ string.Join(" ", eventsUsers.Select(u => MentionHelper.MentionUser(u.Id))) }
");
            }
        }
    }
}
