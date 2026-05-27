namespace DiscordBot.Utils
{
    public static class MentionHelper
    {
        public static string MentionUser(ulong userId)
        {
            return $"<@{userId}>";
        }

        public static string MentionRole(string roleId)
        {
            return $"<@&{roleId}>";
        }
    }
}
