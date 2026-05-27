using DiscordBot.Api.Converters;
using System.Text.Json.Serialization;

namespace DiscordBot.Api.Controllers.Channels.Models
{
    public class ChannelsCacheItemModel
    {
        [JsonConverter(typeof(JsonStringConverter))]
        public ulong ChannelId { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
