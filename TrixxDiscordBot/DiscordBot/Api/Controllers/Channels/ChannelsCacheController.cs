using Discord.WebSocket;
using DiscordBot.Api.Controllers.Channels.Models;
using DiscordBot.MongoDb;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace DiscordBot.Api.Controllers.Channels
{
    public class ChannelsCacheController(
        MongoDbContext mongoDbContext,
        DiscordSocketClient client) : BaseController
    {
        private readonly MongoDbContext _mongoDbContext = mongoDbContext;
        private readonly DiscordSocketClient _client = client;

        [HttpPost("replace")]
        public async Task ReplaceChannels()
        {
            await _mongoDbContext.ChannelsCaches.DeleteManyAsync(_ => true);

            var channels = _client.Guilds.SelectMany(g => g.Channels, (g, c) => new MongoDb.Models.ChannelsCache.ChannelsCache
            {
                ChannelId = c.Id,
                Name = c.Name,
            }).ToList();
            await _mongoDbContext.ChannelsCaches.InsertManyAsync(channels);
        }

        [HttpGet]
        public async Task<IReadOnlyList<ChannelsCacheItemModel>> GetChannels()
        {
            var result = await _mongoDbContext.ChannelsCaches
                .Find(_ => true)
                .Project(c => new ChannelsCacheItemModel
                {
                    ChannelId = c.ChannelId,
                    Name = c.Name
                })
                .ToListAsync();

            return result;
        }
    }
}
