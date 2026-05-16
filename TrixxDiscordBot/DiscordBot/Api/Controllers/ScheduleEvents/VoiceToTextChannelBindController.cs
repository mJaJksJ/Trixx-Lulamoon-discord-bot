using DiscordBot.Api.Controllers.ScheduleEvents.Models;
using DiscordBot.MongoDb;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace DiscordBot.Api.Controllers.ScheduleEvents
{
    public class VoiceToTextChannelBindController(
        MongoDbContext mongoDbContext) : BaseController
    {
        private readonly MongoDbContext _mongoDbContext = mongoDbContext;

        [HttpGet]
        public async Task<IReadOnlyList<VoiceToTextChannelsItemModel>> GetBinds()
        {
            var binds = await _mongoDbContext.VoiceToTextChannels
                .Find(_ => true)
                .ToListAsync();

            var channelIds = binds
                .SelectMany(b => new[] { b.VoiceChannelId, b.TextChannelId })
                .Distinct()
                .ToList();

            var caches = (
                    await _mongoDbContext.ChannelsCaches
                    .Find(c => channelIds.Contains(c.ChannelId))
                    .ToListAsync()
                )
                .ToDictionary(c => c.ChannelId, c => c.Name);

            var nameMap = caches;

            var result = binds.Select(b => new VoiceToTextChannelsItemModel
            {
                VoiceChannelId = b.VoiceChannelId,
                TextChannelId = b.TextChannelId,
                VoiceChannelName = nameMap.TryGetValue(b.VoiceChannelId, out var vName) ? vName : string.Empty,
                TextChannelName = nameMap.TryGetValue(b.TextChannelId, out var tName) ? tName : string.Empty,
            }).ToList();

            return result;
        }

        [HttpPost("replace")]
        public async Task ReplaceBinds(IReadOnlyList<VoiceToTextChannelsUpdateModel> binds)
        {
            await _mongoDbContext.VoiceToTextChannels.DeleteManyAsync(_ => true);

            var newBinds = binds.Select(b => new MongoDb.Models.VoiceToTextChannel
            {
                VoiceChannelId = b.VoiceChannelId,
                TextChannelId = b.TextChannelId,
            }).ToList();
            await _mongoDbContext.VoiceToTextChannels.InsertManyAsync(newBinds);
        }
    }
}
