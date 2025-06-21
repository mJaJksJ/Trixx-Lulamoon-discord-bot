using Microsoft.Extensions.DependencyInjection;
using Trixx.Common.Services;

namespace Trixx.Common
{
    public static class CommonInstaller
    {
        public static IServiceCollection AddCommon(this IServiceCollection services)
        {
            services.AddScoped<LoggerService>();

            return services;
        }
    }
}
