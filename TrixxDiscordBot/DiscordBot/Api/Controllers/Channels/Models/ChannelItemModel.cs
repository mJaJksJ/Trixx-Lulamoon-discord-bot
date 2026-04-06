using Discord;
using DiscordBot.Api.Converters;
using System.Text.Json.Serialization;

namespace DiscordBot.Api.Controllers.Channels.Models
{
    public class ChannelItemModel
    {
        [JsonConverter(typeof(JsonStringConverter))]
        public ulong Id { get; set; }
        public string Name { get; set; } = string.Empty;
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ChannelType Type { get; set; }

        public List<ChannelItemModel> Channels { get; set; } = [];
    }
}
