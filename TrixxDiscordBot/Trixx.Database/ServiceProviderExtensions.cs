using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Trixx.Database
{
    public static class ServiceProviderExtensions
    {
        public static void MigrateHuamDatabase(this IServiceProvider applicationServices)
        {
            using var scope = applicationServices.CreateScope();
            {
                var databaseContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                databaseContext.Database.SetCommandTimeout(TimeSpan.FromHours(1));
                databaseContext.Database.Migrate();
            }
        }
    }
}
