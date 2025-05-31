using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Trixx.Database
{
    public static class TrixxDatabaseInstaller
    {
        public static IServiceCollection AddTrixxDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<IdentityOptions>(options =>
            {
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;
            });

            var connectionString = configuration.GetConnectionString("Main");
            services.AddDbContext<DatabaseContext>(opts =>
            {
                opts.UseNpgsql(connectionString, x => x
                    .MigrationsAssembly("Trixx.Migrations")
                    .SetPostgresVersion(17, 0));
            });

            return services;
        }
    }
}
