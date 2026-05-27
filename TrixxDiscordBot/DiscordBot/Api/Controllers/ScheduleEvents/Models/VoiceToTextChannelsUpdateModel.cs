namespace DiscordBot.Api.Controllers.ScheduleEvents.Models
{
    public class VoiceToTextChannelsUpdateModel
    {
        public ulong VoiceChannelId { get; set; }
        public ulong TextChannelId { get; set; }
    }
}
