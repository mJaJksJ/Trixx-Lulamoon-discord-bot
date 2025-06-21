using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Trixx.Database.Enums;
using TrixxDiscordBot.Server.Startup.Auth;

namespace TrixxDiscordBot.Server.Controllers
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class TrixxClaimsAuthorizeAttribute(Permission permission) : Attribute, IAuthorizationFilter
    {
        internal Permission Permission { get; } = permission;

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;
            if (!user.HasPermission(Permission))
            {
                context.Result = new UnauthorizedResult();
            }
        }
    }
}
