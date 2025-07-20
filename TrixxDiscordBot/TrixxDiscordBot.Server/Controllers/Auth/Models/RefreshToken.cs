namespace TrixxDiscordBot.Server.Controllers.Auth.Models
{
    public sealed class RefreshToken(Guid sessionId, string value)
    {
        public Guid SessionId { get; } = sessionId;
        public string Value { get; } = value;

        internal static RefreshToken? TryDecode(string refreshToken)
        {
            if (string.IsNullOrEmpty(refreshToken))
            {
                return null;
            }
            var parts = refreshToken.Split('|', 2);
            if (parts.Length != 2)
            {
                return null;
            }

            return new RefreshToken(Guid.Parse(parts[0]), parts[1]);
        }

        internal string Encode() => $"{SessionId}|{Value}";
    }
}
