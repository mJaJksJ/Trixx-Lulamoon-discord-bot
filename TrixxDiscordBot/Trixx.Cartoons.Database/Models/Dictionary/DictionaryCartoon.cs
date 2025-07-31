namespace Trixx.Cartoons.Database.Models.Dictionary
{
    public class DictionaryCartoon
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<string> AlternativeNames { get; set; } = [];
        public List<DictionaryCatroonStudio> Studios { get; set; } = [];
        public int Year { get; set; }
        public List<string> Sources { get; set; } = [];
        // TODO: add countries (find list of countries for dictionary)
    }
}
