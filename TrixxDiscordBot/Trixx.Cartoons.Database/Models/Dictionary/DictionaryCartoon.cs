namespace Trixx.Cartoons.Database.Models.Dictionary
{
    public class DictionaryCartoon
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<string> AlternativeNames { get; set; } = [];
        public List<DictionaryCatroonStudio> Studios { get; set; } = [];
        public int? Year { get; set; }
        public List<string> Sources { get; set; } = [];
#pragma warning disable CS8618
        public CartoonSystemObject SystemObject { get; set; }
#pragma warning restore CS8618
        public int SystemObjectId { get; set; }
        // TODO: add countries (find list of countries for dictionary)

        public List<string> NormalizedAllNames { get; set; } = [];
    }
}
