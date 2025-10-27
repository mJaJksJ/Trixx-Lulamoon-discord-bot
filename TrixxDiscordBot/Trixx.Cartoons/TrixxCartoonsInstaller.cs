using Microsoft.Extensions.DependencyInjection;
using Trixx.Cartoons.Services.Pack;

namespace Trixx.Cartoons
{
    public static class TrixxCartoonsInstaller
    {
        public static IServiceCollection AddTrixxCartoons(this IServiceCollection services)
        {
            services.AddScoped<CartoonsPackService>();

            return services;
        }
    }
}
