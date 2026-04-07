using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DiscordBot.MongoDb.Models.ChannelsCache
{
    public class ChannelsCache
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;

        public ulong ChannelId { get; set; }

        public string Name { get; set; } = string.Empty;

        public static void Confiigure(MongoDbContext database)
        {
            database.ChannelsCaches.Indexes.CreateOne(
                new CreateIndexModel<ChannelsCache>(
                    Builders<ChannelsCache>.IndexKeys.Ascending(c => c.ChannelId),
                    new CreateIndexOptions { Unique = true }
                ));
        }
    }
}
