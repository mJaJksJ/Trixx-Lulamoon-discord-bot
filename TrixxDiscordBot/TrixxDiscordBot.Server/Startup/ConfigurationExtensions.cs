using Trixx.Common.Exceptions;

namespace TrixxDiscordBot.Server.Startup
{
    internal static class ConfigurationExtensions
    {
        public static T ReadRequiredSection<T>(this IConfiguration configuration)
            where T : IConfigurationSection
        {
            var section = configuration.GetSection(T.ConfigName).Get<T>();
            return section == null ? throw new TrixxException($"appsettings.json error, section \"{T.ConfigName}\" not found") : section;
        }
    }
}
