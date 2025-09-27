using Microsoft.EntityFrameworkCore;
using Trixx.Core.Services.Roles.Models;
using Trixx.Database;
using Trixx.Database.Models.Identity;

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
                .Where(x => x.Id == id)
                .Select(x => new RoleModel
                {
                    Name = x.Name!
                })
                .FirstAsync();

            return role;
        }
    }
}
