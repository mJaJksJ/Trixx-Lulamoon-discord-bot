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

            var chanelsController = new ChannelsController(_client);
            var channels = chanelsController.GetChannels();
            var channelsCacheItems = new List<MongoDb.Models.ChannelsCache.ChannelsCache>();

            void AddParentAndCheckChilds(ChannelItemModel channelItem)
            {
                channelsCacheItems.Add(new MongoDb.Models.ChannelsCache.ChannelsCache
                {
                    ChannelId = channelItem.Id,
                    Name = channelItem.Name,
                });

                foreach (var child in channelItem.Channels)
                {
                    AddParentAndCheckChilds(child);
                }
            }

            foreach (var channel in channels) 
            {
                AddParentAndCheckChilds(channel);
            }

            await _mongoDbContext.ChannelsCaches.InsertManyAsync(channelsCacheItems);
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
