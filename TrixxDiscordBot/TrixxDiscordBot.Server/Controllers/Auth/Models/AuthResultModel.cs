using System.ComponentModel.DataAnnotations;
using Trixx.Database.Enums;

namespace TrixxDiscordBot.Server.Controllers.Auth.Models
{
    public class AuthResultModel
    {
        [Required]
        public bool Success { get; private set; }
        public string Error { get; private set; } = string.Empty;
        [Required]
        public string AccessToken { get; set; } = string.Empty;
        [Required]
        public string RefreshToken { get; set; } = string.Empty;
        [Required]
        public IEnumerable<Permission> Permissions { get; private set; } = [];
        public AuthFailTypes? FailType { get; private set; }

        internal static AuthResultModel Fail(string error, AuthFailTypes type = AuthFailTypes.Other) => new()
        {
            Success = false,
            Error = error,
            FailType = type,
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
