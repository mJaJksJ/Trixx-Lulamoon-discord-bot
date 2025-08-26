namespace Trixx.Cartoons.Database.Models.Dictionary
{
    public class DictionaryStudio
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
#pragma warning disable CS8618
        public CartoonSystemObject SystemObject { get; set; }
#pragma warning restore CS8618
        public int SystemObjectId { get; set; }
    }
}
