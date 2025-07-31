using Microsoft.EntityFrameworkCore;
using Trixx.Cartoons.Database.Models.Dictionary;

namespace Trixx.Cartoons.Database
{
    public sealed class CartoonsDatabaseContext(DbContextOptions<CartoonsDatabaseContext> options) :
        DbContext(options)
    {
        public DbSet<DictionaryCartoon> DictionaryCartoons { get; set; }
        public DbSet<DictionaryStudio> DictionaryStudios { get; set; }
        public DbSet<DictionaryCatroonStudio> DictionaryCatroonStudios { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(typeof(CartoonsDatabaseContext).Assembly);
        }
    }
}
