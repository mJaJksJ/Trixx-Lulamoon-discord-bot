using Trixx.Cartoons.Database.Enums;
using Trixx.Cartoons.Database.Models.Dictionary;
using Trixx.Database.Common.Models;

namespace Trixx.Cartoons.Database.Models
{
    public class CartoonSystemObject : AbstractSystemObject
    {
        public SystemObjectType Type { get; set; }

#pragma warning disable CS8618
        public DictionaryCartoon DictionaryCartoon { get; set; }
        public int? DictionaryCartoonId { get; set; }

        public DictionaryStudio DictionaryStudio { get; set; }
        public int? DictionaryStudioId { get; set; }
#pragma warning restore CS8618
    }
}
