using DiscordBot.MongoDb.Models;
using DiscordBot.MongoDb.Models.ChannelsCache;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace DiscordBot.MongoDb
{
    public class MongoDbContext(IMongoClient client, IOptions<MongoDbSettings> settings)
    {
        private readonly IMongoDatabase _database = client.GetDatabase(settings.Value.DatabaseName);

        public IMongoCollection<ChannelsCache> ChannelsCaches =>
            _database.GetCollection<ChannelsCache>(nameof(ChannelsCache));

        public IMongoCollection<VoiceToTextChannel> VoiceToTextChannels =>
            _database.GetCollection<VoiceToTextChannel>(nameof(VoiceToTextChannel));
    }
}
