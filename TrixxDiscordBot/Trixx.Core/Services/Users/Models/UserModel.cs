using System.ComponentModel.DataAnnotations;
using Trixx.Common.Models;

namespace Trixx.Core.Services.Users.Models
{
    public class UserModel
    {
        [Required]
        public string UserName { get; set; } = string.Empty;
        [Required]
        public List<SelectItem> Roles { get; set; } = [];
    }
}
