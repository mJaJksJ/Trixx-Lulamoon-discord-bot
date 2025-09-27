using Microsoft.EntityFrameworkCore;
using Trixx.Core.Services.Roles.Models;
using Trixx.Database;
using Trixx.Database.Enums;
using Trixx.Database.Models.Identity;
using Trixx.Database.Utils;

namespace Trixx.Core.Services.Roles
{
    public class RolesService(DatabaseContext databaseContext)
    {
        private readonly DatabaseContext _databaseContext = databaseContext;

        public async Task EditRoleAsync(RoleEditModel model)
        {
            TrixxRole role;

            if (model.Id == null)
            {
                role = new TrixxRole();
            }
            else
            {
                role = await _databaseContext.Roles.FirstAsync(x => x.Id == model.Id);
            }

            role.Name = model.Name;
            role.Permissions = model.Permissions;

            if (model.Id == null)
            {
                _databaseContext.Roles.Add(role);
            }
            await _databaseContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<RoleListItem>> GetRolesAsync()
        {
            var roles = await _databaseContext.Roles
                .Select(x => new RoleListItem
                {
                    Id = x.Id,
                    Label = x.Name!,
                })
                .ToListAsync();

            return roles;
        }

        public async Task<RoleModel> GetRoleAsync(int id)
        {
            var role = await _databaseContext.Roles
                .FirstAsync(r => r.Id == id);

            var allWorkscreen = Enum.GetValues<Workscreen>();
            var allCommonPermissions = Enum.GetValues<CommonPermission>();
            var allPermissions = Enum
                .GetValues(typeof(Permission))
                .Cast<Permission>()
                .Select(x => new
                {
                    Permission = x,
                    Workscreen = x.GetWorkscreen(),
                    CommonPermission = x.GetCommonPermission(),
                })
                .ToList();

            var permissions = new List<WorkscreenPermissionsModel>();
            foreach (var workscreen in allWorkscreen)
            {
                var workscreenPermissions = new WorkscreenPermissionsModel
                {
                    Workscreen = workscreen,
                    Permissions = [],
                };
                foreach (var commonPermission in allCommonPermissions)
                {
                    var permission = allPermissions.FirstOrDefault(x => x.Workscreen == workscreen && x.CommonPermission == commonPermission)?.Permission;
                    workscreenPermissions.Permissions.Add(new PermissionModel
                    {
                        Id = permission,
                        CommonPermission = commonPermission,
                        IsGranted = permission.HasValue ? role.Permissions.Contains(permission.Value) : null,
                    });
                }
                permissions.Add(workscreenPermissions);
            }

            return new RoleModel
            {
                Name = role.Name!,
                Permissions = permissions
            };
        }
    }
}
