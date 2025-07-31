using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Trixx.Cartoons.Database;

namespace Trixx.Cartoons.Database
{
    public static class TrixxCartoonsDatabaseInstaller
    {
        public static IServiceCollection AddTrixxCartoonsDatabases(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<IdentityOptions>(options =>
            {
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;
            });

            var connectionString = configuration.GetConnectionString("Cartoons");
            services.AddDbContext<CartoonsDatabaseContext>(opts =>
            {
                opts.UseNpgsql(connectionString, x => x
                    .MigrationsAssembly("Trixx.Cartoons.Migrations")
                    .SetPostgresVersion(17, 0));
            });

            return services;
        }

        public static void MigrateTrixxCartoonsDatabase(this IServiceProvider applicationServices)
        {
            using var scope = applicationServices.CreateScope();
            {
                var databaseContext = scope.ServiceProvider.GetRequiredService<CartoonsDatabaseContext>();
                databaseContext.Database.SetCommandTimeout(TimeSpan.FromHours(1));
                databaseContext.Database.Migrate();
            }
        }
    }
}
