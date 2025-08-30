using Trixx.Database.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Trixx.Database.Models.FormDefaults;

namespace Trixx.Database
{
    public sealed class DatabaseContext(DbContextOptions<DatabaseContext> options) :
        IdentityDbContext<TrixxUser, TrixxRole, int, IdentityUserClaim<int>, TrixxUserRole, IdentityUserLogin<int>, IdentityRoleClaim<int>, IdentityUserToken<int>>(options)
    {
#pragma warning disable CS8618
        public DbSet<FormDeafault> FormDeafaults { get; set; }
#pragma warning restore CS8618

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(typeof(DatabaseContext).Assembly);
        }
    }
}
