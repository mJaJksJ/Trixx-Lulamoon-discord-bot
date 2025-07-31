namespace Trixx.Cartoons.Database.Models.Dictionary
{
    public class DictionaryCatroonStudio
    {
        public int Id { get; set; }
#pragma warning disable CS8618
        public DictionaryCartoon Cartoon { get; set; }
        public int CartoonId { get; set; }
        public DictionaryStudio DictionaryStudio { get; set; }
        public int DictionaryStudioId { get; set; }
#pragma warning restore CS8618
    }
}
