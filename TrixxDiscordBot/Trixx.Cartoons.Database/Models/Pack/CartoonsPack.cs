namespace Trixx.Cartoons.Database.Models.Pack
{
    public class CartoonsPack
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<CartoonsPackLabelType> CartoonsPackLabelTypes { get; set; } = [];

#pragma warning disable CS8618
        public CartoonSystemObject SystemObject { get; set; }
        public int SystemObjectId { get; set; }
#pragma warning restore CS8618
    }
}
