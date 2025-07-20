using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Trixx.Common.Exceptions;
using Trixx.Database;
using TrixxDiscordBot.Server.Controllers.UserProfile.Models;
using TrixxDiscordBot.Server.Startup.Auth;

namespace TrixxDiscordBot.Server.Controllers.UserProfile
{
    public class UserProfileController(DatabaseContext databaseContext) : ApiController
    {
        private readonly DatabaseContext _databaseContext = databaseContext;

        [HttpGet]
        public async Task<UserProfileModel> GetUserProfileAsync()
        {
            var userId = HttpContext.User.GetId();
            var result = await _databaseContext.Users
                .Where(x => x.Id == userId)
                .Select(x => new UserProfileModel
                {
                    Name = !string.IsNullOrEmpty(x.UserName) ? x.UserName : string.Empty
                })
                .FirstOrDefaultAsync();

            return result ?? throw new TrixxException("Информация о пользователе не найдена");
        }
    }
}
