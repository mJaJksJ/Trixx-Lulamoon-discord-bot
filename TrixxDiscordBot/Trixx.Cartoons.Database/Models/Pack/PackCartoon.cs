using Trixx.Cartoons.Database.Models.Dictionary;

namespace Trixx.Cartoons.Database.Models.Pack
{
    public class PackCartoon
    {
        public int Id { get; set; }

#pragma warning disable CS8618
        public DictionaryCartoon DictionaryCartoon { get; set; }
        public int DictionaryCartoonId { get; set; }

        public CartoonsPackLabelType CartoonsPackLabelType { get; set; }
        public int CartoonsPackLabelTypeId { get; set; }

        public CartoonSystemObject SystemObject { get; set; }
        public int SystemObjectId { get; set; }
#pragma warning restore CS8618
    }
}
