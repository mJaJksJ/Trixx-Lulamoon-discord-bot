using System.ComponentModel.DataAnnotations;
using Trixx.Database.Enums;

namespace TrixxDiscordBot.Server.Controllers.Auth.Models
{
    public class AuthResultModel
    {
        [Required]
        public bool Success { get; private set; }
        public string Error { get; private set; } = string.Empty;
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public IEnumerable<Permission> Permissions { get; private set; } = [];

        internal static AuthResultModel Fail(string error) => new()
        {
            Success = false,
            Error = error,
        };

        internal static AuthResultModel Ok(string accessToken, string refreshToken, IEnumerable<Permission> permissions) => new()
        {
            Success = true,
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            Permissions = permissions,
        };
    }
}
