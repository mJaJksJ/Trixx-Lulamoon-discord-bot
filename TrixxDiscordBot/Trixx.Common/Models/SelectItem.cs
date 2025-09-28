using System.ComponentModel.DataAnnotations;

namespace Trixx.Common.Models
{
    public class SelectItem
    {
        [Required]
        public int Id { get; set; }
        public string Label { get; set; } = string.Empty;
    }
}
