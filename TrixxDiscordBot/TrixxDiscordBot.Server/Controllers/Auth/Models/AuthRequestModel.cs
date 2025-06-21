using System.ComponentModel.DataAnnotations;

namespace TrixxDiscordBot.Server.Controllers.Auth.Models
{
    public class AuthRequestModel
    {
        [Required]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
