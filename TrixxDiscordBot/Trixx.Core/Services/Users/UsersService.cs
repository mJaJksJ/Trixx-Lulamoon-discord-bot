using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Trixx.Common.Utils;
using Trixx.Core.Services.Users.Models;
using Trixx.Database;
using Trixx.Database.Enums;
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
                    UserName = x.UserName!
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
    }
}
