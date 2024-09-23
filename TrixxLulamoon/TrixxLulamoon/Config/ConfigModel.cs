namespace TrixxLulamoon.Config
{
    public class ConfigModel
    {
        public static ConfigModel Instance { get; private set; }

        public DiscordConfig DiscordConfig { get; set; }
        public RolesConfig RolesConfig { get; set; }

        public static void Create(ConfigurationManager configuration) 
        {
            Instance = configuration.Get<ConfigModel>();
        }
    }
}
