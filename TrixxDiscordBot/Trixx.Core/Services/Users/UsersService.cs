using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Trixx.Common.Models;
using Trixx.Common.Utils;
using Trixx.Core.Services.Users.Models;
using Trixx.Database;
using Trixx.Database.Models.Identity;
using TrixxCore.Services.Users.Models;

namespace Trixx.Core.Services.Users
{
    public class UsersService(DatabaseContext databaseContext)
    {
        private readonly DatabaseContext _databaseContext = databaseContext;

        public async Task CreateUserManuallyAsync(UserManuallyCreateModel model)
        {
            var normalizedUserName = model.UserName.ToNormalized();
            var email = $"{model.UserName}@trixx.trixx";
            var normalizedEmail = email.ToNormalized();

            var user = new TrixxUser
            {
                Email = model.UserName,
                NormalizedEmail = normalizedUserName,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.UserName,
                FullName = model.UserName,
                NormalizedUserName = normalizedUserName,
                PasswordHash = new PasswordHasher<TrixxUser>().HashPassword(null!, "trixx"),
                ConcurrencyStamp = Guid.NewGuid().ToString(),
                EmailConfirmed = false,
                LockoutEnabled = true,
            };

            _databaseContext.Users.Add(user);
            await _databaseContext.SaveChangesAsync();

            var rolesMerger = new ManyToManyDbMerger<TrixxUserRole>(_databaseContext);
            await rolesMerger.MergeAsync(
                model.Roles,
                x => false,
                (x, i) => x.UserId == i,
                i => new TrixxUserRole
                {
                    UserId = user.Id,
                    RoleId = i,
                }
            );
            await _databaseContext.SaveChangesAsync();
        }

        public async Task UpdateUserAsync(UpdateUserModel model)
        {
            var user = await _databaseContext.Users
                .FirstAsync(x => x.Id == model.Id);

            if (!string.IsNullOrEmpty(model.Password))
            {
                user.PasswordHash = new PasswordHasher<TrixxUser>().HashPassword(user, model.Password);
            }
            await _databaseContext.SaveChangesAsync();

            var rolesMerger = new ManyToManyDbMerger<TrixxUserRole>(_databaseContext);
            await rolesMerger.MergeAsync(
                model.Roles,
                x => x.UserId == model.Id,
                (x, i) => x.RoleId == i,
                i => new TrixxUserRole
                {
                    UserId = user.Id,
                    RoleId = i,
                }
            );
            await _databaseContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<UserListItem>> GetUsersAsync()
        {
            var users = await _databaseContext.Users
                .Select(x => new UserListItem
                {
                    Id = x.Id,
                    Label = x.FullName + " (" + x.UserName + ")",
                    IsLocked = x.LockoutEnd.HasValue && x.LockoutEnd.Value > DateTimeOffset.UtcNow,
                    IsLockable = x.LockoutEnabled,
                })
                .ToListAsync();

            return users;
        }

        public async Task<UserModel> GetUserAsync(int id)
        {
            var user = await _databaseContext.Users
                .Where(x => x.Id == id)
                .Select(x => new UserModel
                {
                    UserName = x.UserName!,
                    Roles = x.Roles.Select(r => new Common.Models.SelectItem
                    {
                        Id = r.RoleId,
                        Label = r.Role.Name!
                    }).ToList()
                })
                .FirstAsync();

            return user;
        }

        public async Task ChangeUserLockStatusAsync(int userId, bool toLock)
        {
            var user = await _databaseContext.Users
                .FirstAsync(x => x.Id == userId);
            user.LockoutEnd = toLock ? DateTimeOffset.MaxValue : null;
            await _databaseContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<SelectItem>> GetRolesAsync()
        {
            var roles = await _databaseContext.Roles
                .Select(x => new SelectItem
                {
                    Id = x.Id,
                    Label = x.Name!,
                })
                .ToListAsync();

            return roles;
        }
    }
}
