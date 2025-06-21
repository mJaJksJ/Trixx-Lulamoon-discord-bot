using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Trixx.Database.Enums;

namespace TrixxDiscordBot.Server.Startup.Auth
{
    public sealed class JwtBuilder
    {
        private readonly JwtConfiguration _jwtConfiguration;
        private readonly SigningCredentials _signingCredentials;
        private readonly JwtSecurityTokenHandler _tokenHandler;

        public JwtBuilder(JwtConfiguration jwtConfiguration)
        {
            _jwtConfiguration = jwtConfiguration;
            _signingCredentials = new SigningCredentials(
                _jwtConfiguration.SigningKey,
                SecurityAlgorithms.HmacSha256Signature);
            _tokenHandler = new JwtSecurityTokenHandler();
        }

        internal string Build(UserData userData, Guid sessionId)
        {
            var claims = GenerateClaimsForUser(userData, sessionId);

            var token = new JwtSecurityToken(
                issuer: _jwtConfiguration.IssuerFullName,
                expires: DateTime.Now.AddMinutes(_jwtConfiguration.AccessTokenLifetimeMinutes),
                claims: claims,
                audience: _jwtConfiguration.Audience,
                signingCredentials: _signingCredentials);

            return _tokenHandler.WriteToken(token);
        }

        internal static List<Claim> GenerateClaimsForUser(UserData userData, Guid sessionId)
        {
            var result = new List<Claim>();
            result.AddRange(userData.Permissions.Select(p => p.ToClaim()));
            result.Add(new Claim(TrixxClaimTypes.CLAIMTYPE_USER_ID, userData.UserId.ToString()));
            result.Add(new Claim(TrixxClaimTypes.CLAIMTYPE_SESSION_ID, sessionId.ToString()));

            if (userData.IsAdmin)
            {
                result.Add(new Claim(TrixxClaimTypes.CLAIMTYPE_IS_ADMIN, TrixxClaimTypes.CLAIMTYPE_IS_ADMIN_VALUE));
            }

            return result;
        }

        internal static ClaimsPrincipal BuildUser(UserData userData, Guid sessionId)
        {
            var claims = GenerateClaimsForUser(userData, sessionId);
            var identity = new ClaimsIdentity(claims);
            return new ClaimsPrincipal(identity);
        }

        internal record UserData(int UserId, IEnumerable<Permission> Permissions, bool IsAdmin);
    }
}
