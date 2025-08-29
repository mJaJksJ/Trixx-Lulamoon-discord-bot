using Trixx.Cartoons.Database.Models.Dictionary;
using Trixx.Common.Models;

namespace TrixxDiscordBot.Server.Controllers.Cartoons.DictionaryCartoons.Models
{
    public class CartoonsListSelectItem : SelectItem
    {
        public List<string> AlternativeNames { get; set; } = [];
        public List<SelectItem> Studios { get; set; } = [];
        public int? Year { get; set; }
        public List<RefItem> Sources { get; set; } = [];
    }
}
