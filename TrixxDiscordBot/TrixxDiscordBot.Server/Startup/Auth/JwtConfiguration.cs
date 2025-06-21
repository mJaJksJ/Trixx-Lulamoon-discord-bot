using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace TrixxDiscordBot.Server.Startup.Auth
{
    public class JwtConfiguration : IConfigurationSection
    {
        private string _secret = "secret";
        private string _issuer = "issuer";

        public static string ConfigName => "Jwt";

        public string Secret
        {
            get => _secret;
            set
            {
                _secret = value;
                SigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(value));
            }
        }

        public int AccessTokenLifetimeMinutes { get; set; }
        public int RefreshTokenLifetimeDays { get; set; }
        public string? Audience { get; set; }
        public string Issuer
        {
            get => _issuer; 
            set
            {
                _issuer = value;
                IssuerFullName = _issuer + "." + typeof(JwtConfiguration).Assembly.GetName().Version?.ToString();
            }
        }
        public string? IssuerFullName { get; private set; }
        public SecurityKey? SigningKey { get; private set; }
    }
}
