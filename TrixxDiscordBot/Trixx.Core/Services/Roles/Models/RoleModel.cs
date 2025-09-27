using System.ComponentModel.DataAnnotations;
using Trixx.Database.Enums;

namespace Trixx.Core.Services.Roles.Models
{
    public class RoleModel
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public List<WorkscreenPermissionsModel> Permissions { get; set; } = [];
    }

    public class WorkscreenPermissionsModel
    {
        [Required]
        public Workscreen Workscreen { get; set; }
        [Required]
        public List<PermissionModel> Permissions { get; set; } = [];
    }

    public class PermissionModel
    {
        public Permission? Id { get; set; }
        [Required]
        public CommonPermission CommonPermission { get; set; }
        public bool? IsGranted { get; set; }
    }
}
