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
        private readonly IConfiguration _configuration;

        public InteractionHandler(
            DiscordSocketClient client,
            InteractionService interactions,
            IServiceProvider services,
            MongoDbContext mongoDbContext,
            IConfiguration configuration
            )
        {
            _interactions = interactions;
            _client = client;
            _services = services;
            _mongoDbContext = mongoDbContext;
            _configuration = configuration;
        }

        public async Task InstallInteractionsAsync()
        {
            await _interactions.AddModulesAsync(
                assembly: Assembly.GetEntryAssembly(),
                services: _services);

            _client.InteractionCreated += HandleInteractionAsync;
            _client.Ready += ReadyAsync;

            _client.GuildScheduledEventUpdated += 
                async (before, after) => await ScheduleEventsInteractionHelper.ScheduleEventUpdatedAsync(before, after, _client, _mongoDbContext);
        }

        private async Task HandleInteractionAsync(SocketInteraction interaction)
        {
            try
            {
                var context = new SocketInteractionContext(_client, interaction);
                var result = await _interactions.ExecuteCommandAsync(context, _services);

                if (!result.IsSuccess)
                {
                    //TODO: add logger
                }
            }
            catch (Exception ex)
            {
                //TODO: add logger
            }
        }

        private async Task ReadyAsync()
        {
            var guildId = _configuration.GetValue<ulong>("Discord:GuildId");
            await _interactions.RegisterCommandsToGuildAsync(guildId);
        }
    }
}
