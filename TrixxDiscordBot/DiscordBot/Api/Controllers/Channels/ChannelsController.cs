using Discord;
using Discord.WebSocket;
using DiscordBot.Api.Controllers.Channels.Models;
using Microsoft.AspNetCore.Mvc;

namespace DiscordBot.Api.Controllers.Channels
{
    public class ChannelsController(DiscordSocketClient client) : BaseController
    {
        private readonly DiscordSocketClient _client = client;

        [HttpGet("channels")]
        public List<ChannelItemModel> GetChannels()
        {
            var channels = _client.Guilds.SelectMany(g => g.Channels).ToList();

            var categories = channels
                .OfType<SocketCategoryChannel>()
                .OrderBy(c => c.Position)
                .ToList();

            var uncategorized = channels
                .Where(c =>
                    c is SocketTextChannel stc && stc.CategoryId == null ||
                    c is SocketVoiceChannel svc && svc.CategoryId == null ||
                    c is SocketStageChannel ssc && ssc.CategoryId == null)
                .OrderBy(c => c.Position)
                .Select(c => new ChannelItemModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    Type = c.ChannelType,
                })
                .ToList();

            var result = new List<ChannelItemModel>();
            result.AddRange(uncategorized);

            foreach (var category in categories)
            {
                var channelsInCategory = channels
                    .Where(c =>
                        c is SocketTextChannel stc && stc.CategoryId == category.Id ||
                        c is SocketVoiceChannel svc && svc.CategoryId == category.Id ||
                        c is SocketStageChannel ssc && ssc.CategoryId == category.Id)
                    .OrderBy(c => c.Position)
                    .Select(c => new ChannelItemModel
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Type = c.ChannelType,
                        Channels = []
                    })
                    .ToList();

                foreach (var channelInCategory in channelsInCategory)
                {
                    var threads = channels
                        .OfType<SocketThreadChannel>()
                        .Where(t => t.ParentChannel.Id == channelInCategory.Id)
                        .OrderBy(c => c.Position)
                        .Select(c => new ChannelItemModel
                        {
                            Id = c.Id,
                            Name = c.Name,
                            Type = c.ChannelType,
                            Channels = []
                        })
                        .ToList();

                    channelInCategory.Channels.AddRange(threads);

                }

                var categoryModel = new ChannelItemModel
                {
                    Id = category.Id,
                    Name = category.Name,
                    Type = ChannelType.Category,
                    Channels = channelsInCategory
                };

                result.Add(categoryModel);
            }

            return result;
        }
    }
}
