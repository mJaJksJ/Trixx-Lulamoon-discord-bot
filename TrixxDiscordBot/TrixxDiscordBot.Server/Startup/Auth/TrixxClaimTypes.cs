using Trixx.Database.Models.Identity;

namespace TrixxDiscordBot.Server.Startup.Auth
{
    public class TrixxClaimTypes
    {
        internal const string CLAIMTYPE_PERMISSION = "trixx-permission";
        internal const string CLAIMTYPE_USER_ID = "trixx-user-id";
        internal const string CLAIMTYPE_SESSION_ID = "trixx-session-id";
        internal const string CLAIMTYPE_IS_ADMIN = "trixx-is-admin";
        internal static readonly string CLAIMTYPE_IS_ADMIN_VALUE = $"trixx-{TrixxRole.ID_ADMIN}";
    }
}
