using System.ComponentModel.DataAnnotations;

namespace TrixxDiscordBot.Server.Controllers.UserProfile.Models
{
    public class UserProfileModel
    {
        [Required]
        public string Name { get; internal set; } = string.Empty;
    }
}
