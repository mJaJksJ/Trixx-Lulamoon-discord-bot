using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DiscordBot.MongoDb.Models
{
    public class VoiceToTextChannel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;

        public ulong VoiceChannelId { get; set; }
        public ulong TextChannelId { get; set; }

        public static void Confiigure(MongoDbContext database)
        {
            database.VoiceToTextChannels.Indexes.CreateOne(
                new CreateIndexModel<VoiceToTextChannel>(
                    Builders<VoiceToTextChannel>.IndexKeys.Ascending(c => new { c.VoiceChannelId, c.TextChannelId }),
                    new CreateIndexOptions { Unique = true }
                ));
        }
    }
}
