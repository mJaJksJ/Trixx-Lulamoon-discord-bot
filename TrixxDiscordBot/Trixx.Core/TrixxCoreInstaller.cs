using Microsoft.Extensions.DependencyInjection;
using Trixx.Core.Services.Users;

namespace Trixx.Core
{
    public static class TrixxCoreInstaller
    {
        public static IServiceCollection AddTrixxCore(this IServiceCollection services)
        {
            services.AddScoped<UsersService>();

            return services;
        }
    }
}
