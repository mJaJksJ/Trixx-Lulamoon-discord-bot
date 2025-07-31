namespace TrixxDiscordBot.Server.Controllers.Cartoons.DictionaryStudios.Models
{
    public class StudioUpdateModel
    {
        public int? Id { get; set; } // null if create
        public string Name { get; set; } = string.Empty;
    }
}
