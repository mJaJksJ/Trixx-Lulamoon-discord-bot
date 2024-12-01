namespace TrixxLulamoon.Utils
{
    public static class MentionHelper
    {
        public static string MentionUser(string userId)
        {
            return $"<@{userId}>";
        }

        public static string MentionRole(string roleId)
        {
            return $"<@&{roleId}>";
        }
    }
}
