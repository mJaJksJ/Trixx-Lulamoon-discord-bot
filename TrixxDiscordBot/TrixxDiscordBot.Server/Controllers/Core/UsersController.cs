using Microsoft.AspNetCore.Mvc;
using Trixx.Core.Services.Users;
using Trixx.Core.Services.Users.Models;
using Trixx.Database;
using Trixx.Database.Enums;
using TrixxCore.Services.Users.Models;

namespace TrixxDiscordBot.Server.Controllers.Core
{
    public class UsersController(DatabaseContext databaseContext, UsersService usersService) : ApiController
    {
        private readonly DatabaseContext _databaseContext = databaseContext;
        private readonly UsersService _usersService = usersService;

        [HttpPost("create-user-manually")]
        [TrixxClaimsAuthorize(Permission.TrixxUsers_Edit)]
        public async Task CreateUserManuallyAsync(UserManuallyCreateModel model)
        {
            await _usersService.CreateUserManuallyAsync(model);
        }

        [HttpGet("users")]
        [TrixxClaimsAuthorize(Permission.TrixxUsers_Read)]
        public async Task<IEnumerable<UserListItem>> GetUsersAsync()
        {
            return await _usersService.GetUsersAsync();
        }

        [HttpGet("{id}")]
        [TrixxClaimsAuthorize(Permission.TrixxUsers_Read)]
        public async Task<UserModel> GetUserAsync(int id)
        {
            return await _usersService.GetUserAsync(id);
        }

        [HttpPost("change-user-lock-status/{userId}")]
        [TrixxClaimsAuthorize(Permission.TrixxUsers_Edit)]
        public async Task ChangeUserLockStatusAsync(int userId, [FromQuery] bool toLock)
        {
            await _usersService.ChangeUserLockStatusAsync(userId, toLock);
        }
    }
}
