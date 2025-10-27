using System.ComponentModel.DataAnnotations;
using Trixx.Cartoons.Database.Enums;

namespace Trixx.Cartoons.Services.Pack.Models
{
    public class CartoonsPackModel
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public List<LabelType> LabelTypes { get; set; } = [];

        public class LabelType
        {
            [Required]
            public int Id { get; set; }
            public string Name { get; set; } = string.Empty;
            [Required]
            public List<CartoonItem> Cartoons { get; set; } = [];
            [Required]
            public int Order { get; set; }
        }

        public class CartoonItem
        {
            [Required]
            public int DictionaryCartoonId { get; set; }
            public string Name { get; set; } = string.Empty;
            public CartoonType CartoonType { get; set; }
            public int? Year { get; set; }
            public string AlternativeNames { get; set; } = string.Empty;
        }
    }
}
