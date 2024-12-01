using Discord.Interactions;
using Discord.WebSocket;
using System.Reflection;
using TrixxLulamoon.Interactions.Admin;
using TrixxLulamoon.Interactions.Users;

namespace TrixxLulamoon
{
    public class InteractionHandler
    {
        private readonly DiscordShardedClient _client;
        private readonly InteractionService _interactions;
        private readonly IServiceProvider _services;
        private readonly AdminButtonsHandler _adminButtonsHandler;
        private readonly AdminModalsHandler _adminModalsHandler;
        private readonly UsersButtonsHandler _usersButtonsHandler;

        public InteractionHandler(
            DiscordShardedClient client,
            InteractionService interactions,
            IServiceProvider services,
            AdminButtonsHandler adminButtonsHandler,
            AdminModalsHandler adminModalsHandler,
            UsersButtonsHandler usersButtonsHandler
            )
        {
            _interactions = interactions;
            _client = client;
            _services = services;
            _adminButtonsHandler = adminButtonsHandler;
            _adminModalsHandler = adminModalsHandler;
            _usersButtonsHandler = usersButtonsHandler;
        }

        public async Task InstallCommandsAsync()
        {
            _client.ButtonExecuted += _adminButtonsHandler.ButtonExecuted;
            _client.ButtonExecuted += _usersButtonsHandler.ButtonExecuted;
            _client.ModalSubmitted += _adminModalsHandler.ModalSubmitted;

            await _interactions.AddModulesAsync(
                assembly: Assembly.GetEntryAssembly(),
                services: _services);
        }
    }
}
