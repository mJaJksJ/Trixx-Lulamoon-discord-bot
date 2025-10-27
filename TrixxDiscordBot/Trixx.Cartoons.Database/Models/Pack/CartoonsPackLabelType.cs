namespace Trixx.Cartoons.Database.Models.Pack
{
    public class CartoonsPackLabelType
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Order { get; set; }

#pragma warning disable CS8618
        public CartoonsPack CartoonsPack { get; set; }
        public int CartoonsPackId { get; set; }

        public List<PackCartoon> PackCartoons { get; set; }

        public CartoonSystemObject SystemObject { get; set; }
        public int SystemObjectId { get; set; }
#pragma warning restore CS8618
    }
}
