using Trixx.Common.Services;
using Trixx.Common.Utils;
using Trixx.Database;
using Trixx.Database.Models.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrixxDiscordBot.Server.Controllers.Auth.Models;
using TrixxDiscordBot.Server.Startup.Auth;

namespace TrixxDiscordBot.Server.Controllers.Auth
{
    [AllowAnonymous]
    public class AuthController(
        SignInManager<TrixxUser> signInManager,
        UserManager<TrixxUser> userManager,
        JwtBuilder jwtBuilder,
        DatabaseContext databaseContext,
        LoggerService loggerService) : ApiController
    {
        private const string RefreshTokenString = "RefreshToken";
        private readonly SignInManager<TrixxUser> _signInManager = signInManager;
        private readonly UserManager<TrixxUser> _userManager = userManager;
        private readonly JwtBuilder _jwtBuilder = jwtBuilder;
        private readonly DatabaseContext _databaseContext = databaseContext;
        private readonly LoggerService _logger = loggerService;

        [HttpPost("login")]
        public async Task<AuthResultModel> LoginAsync([FromBody] AuthRequestModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                _logger.Error($"Fail to auth user {model.Email} - user not found");
                return AuthResultModel.Fail("Неверное имя пользователя");
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, true);
            if (!result.Succeeded)
            {
                if (result.IsLockedOut)
                {
                    if (user.LockoutEnd.HasValue && user.LockoutEnd.Value.Date == DateTimeOffset.MaxValue.Date)
                    {
                        _logger.Error($"Fail to auth user {user.UserName} - user lockout");
                        return AuthResultModel.Fail($"Пользователь заблокирован");
                    }
                    _logger.Error($"Fail to auth user {user.UserName} - user lockout");
                    return AuthResultModel.Fail($"Пользователь заблокирован до {user.LockoutEnd.As_ddMMyyyy()}");
                }

                if (result.IsNotAllowed)
                {
                    _logger.Error($"Fail to auth user {user.UserName} - user not allowed");
                    return AuthResultModel.Fail("Доступ запрещён");
                }

                _logger.Error($"Fail to auth user {user.UserName} - bad password");
                return AuthResultModel.Fail("Неверный пароль");
            }

            _logger.Info($"User {user.UserName} login");
            return await SuccessLoginAsync(user);
        }

        [HttpDelete("logout")]
        public async Task LogoutAsync()
        {
            var userId = HttpContext.User.GetId();
            var user = await _databaseContext.Users.FirstOrDefaultAsync(x => x.Id == userId);

            if (user == null)
            {
                return;
            }

            await _userManager.RemoveAuthenticationTokenAsync(user, TrixxAuthExtensions.RefreshTokenProviderName, RefreshTokenString);
            await _signInManager.SignOutAsync();
        }

        [HttpPost("refresh")]
        public async Task<AuthResultModel> RefreshAsync(int userId, string refreshToken)
        {
            var user = await _databaseContext.Users.SingleOrDefaultAsync(x => x.Id == userId);
            if (user == null)
            {
                return AuthResultModel.Fail("Пользователь в системе не найден");
            }
            var rt = RefreshToken.TryDecode(refreshToken);
            if (rt == null)
            {
                return AuthResultModel.Fail("Слишком долгое бездействие. Необходима повторная аутентификация", AuthFailTypes.RefreshTokenInvalid);
            }

            if (await _userManager.IsLockedOutAsync(user))
            {
                return AuthResultModel.Fail("Учетная запись заблокирована");
            }

            var refreshTokenFromDb = await _userManager.GetAuthenticationTokenAsync(user, TrixxAuthExtensions.RefreshTokenProviderName, RefreshTokenString);
            if (refreshTokenFromDb != rt.Value || !await _userManager.VerifyUserTokenAsync(user, TrixxAuthExtensions.RefreshTokenProviderName, RefreshTokenString, refreshTokenFromDb))
            {
                return AuthResultModel.Fail("Слишком долгое бездействие. Необходима повторная аутентификация", AuthFailTypes.RefreshTokenInvalid);
            }

            var userData = await GetUserDataAsync(user.Id);
            var newAccessToken = _jwtBuilder.Build(userData, rt.SessionId);

            string newRefreshToken = await CreateNewRefreshTokenAsync(user, rt.SessionId);

            return AuthResultModel.Ok(newAccessToken, newRefreshToken, userData.Permissions);
        }

        private async Task<AuthResultModel> SuccessLoginAsync(TrixxUser user)
        {
            var userData = await GetUserDataAsync(user.Id);
            var sessionId = Guid.NewGuid();

            var accessToken = _jwtBuilder.Build(userData, sessionId);
            HttpContext.User = JwtBuilder.BuildUser(userData, sessionId);
            string newRefreshToken = await CreateNewRefreshTokenAsync(user, sessionId);

            return AuthResultModel.Ok(accessToken, newRefreshToken, userData.Permissions);
        }

        private async Task<JwtBuilder.UserData> GetUserDataAsync(int userId)
        {
            var roles = await _databaseContext.UserRoles
                .Where(x => x.UserId == userId)
                .Join(_databaseContext.Roles, x => x.RoleId, x => x.Id, (l, r) => new { r.Permissions, l.RoleId })
                .ToListAsync();

            var permissions = roles.SelectMany(x => x.Permissions).Distinct().ToList();
            var isAdmin = roles.Any(s => s.RoleId == TrixxRole.ID_ADMIN);

            return new JwtBuilder.UserData(userId, permissions, isAdmin);
        }

        private async Task<string> CreateNewRefreshTokenAsync(TrixxUser user, Guid sessionId)
        {
            await _userManager.RemoveAuthenticationTokenAsync(user, TrixxAuthExtensions.RefreshTokenProviderName, RefreshTokenString);
            var newRefreshToken = await _userManager.GenerateUserTokenAsync(user, TrixxAuthExtensions.RefreshTokenProviderName, RefreshTokenString);
            await _userManager.SetAuthenticationTokenAsync(user, TrixxAuthExtensions.RefreshTokenProviderName, RefreshTokenString, newRefreshToken);

            return new RefreshToken(sessionId, newRefreshToken).Encode();
        }
    }
}
