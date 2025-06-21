namespace TrixxDiscordBot.Server.Startup
{
    public interface IConfigurationSection
    {
        static abstract string ConfigName { get; }
    }
}
