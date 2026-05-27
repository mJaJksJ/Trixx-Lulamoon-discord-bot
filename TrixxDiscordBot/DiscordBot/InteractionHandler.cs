using Discord.Interactions;
using Discord.WebSocket;
using DiscordBot.MongoDb;
using DiscordBot.Utils.InteractionUtils;
using System.Reflection;

namespace DiscordBot
{
    public class InteractionHandler
    {
        private readonly DiscordSocketClient _client;
        private readonly InteractionService _interactions;
        private readonly IServiceProvider _services;
        private readonly MongoDbContext _mongoDbContext;

        public InteractionHandler(
            DiscordSocketClient client,
            InteractionService interactions,
            IServiceProvider services,
            MongoDbContext mongoDbContext
            )
        {
            _interactions = interactions;
            _client = client;
            _services = services;
            _mongoDbContext = mongoDbContext;
        }

        public async Task InstallInteractionsAsync()
        {
            await _interactions.AddModulesAsync(
                assembly: Assembly.GetEntryAssembly(),
                services: _services);

            _client.GuildScheduledEventUpdated += 
                async (before, after) => await ScheduleEventsInteractionHelper.ScheduleEventUpdatedAsync(before, after, _client, _mongoDbContext);
        }
    }
}
