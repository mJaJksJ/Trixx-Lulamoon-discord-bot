using Trixx.Common.Models;

namespace TrixxDiscordBot.Server.Controllers.Cartoons.DictionaryCartoons.Models
{
    public class CartoonUpdateModel
    {
        public int? Id { get; set; } // null if create
        public string Name { get; set; } = string.Empty;
        public int? Year { get; set; }
        public List<string> AlternativeNames { get; set; } = [];
        public List<int> Studios { get; set; } = [];
        public List<string> Sources { get; set; } = [];
    }
}
