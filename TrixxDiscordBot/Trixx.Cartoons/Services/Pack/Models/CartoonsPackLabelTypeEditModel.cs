namespace Trixx.Cartoons.Services.Pack.Models
{
    public class CartoonsPackLabelTypeEditModel
    {
        public int? Id { get; set; }
        public int CartoonPackId {  get; set; }
        public string Name { get; set; } = string.Empty;
        public int Order { get; set; }
    }
}
