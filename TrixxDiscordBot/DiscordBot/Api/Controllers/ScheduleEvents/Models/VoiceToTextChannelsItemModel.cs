namespace DiscordBot.Api.Controllers.ScheduleEvents.Models
{
    public class VoiceToTextChannelsItemModel : VoiceToTextChannelsUpdateModel
    {
        public string VoiceChannelName { get; set; } = string.Empty;
        public string TextChannelName { get; set; } = string.Empty;
    }
}
