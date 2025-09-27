using Trixx.Database.Enums;

namespace Trixx.Core.Services.Roles.Models
{
    public class RoleEditModel
    {
        public int? Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<Permission> Permissions { get; set; } = [];
    }
}
