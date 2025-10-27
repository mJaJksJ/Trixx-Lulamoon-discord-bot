using Microsoft.EntityFrameworkCore;
using Trixx.Cartoons.Database.Models.Dictionary;
using Trixx.Cartoons.Database.Models.Pack;

namespace Trixx.Cartoons.Database
{
    public sealed class CartoonsDatabaseContext(DbContextOptions<CartoonsDatabaseContext> options) :
        DbContext(options)
    {
        public DbSet<DictionaryCartoon> DictionaryCartoons { get; set; }
        public DbSet<DictionaryStudio> DictionaryStudios { get; set; }
        public DbSet<DictionaryCatroonStudio> DictionaryCatroonStudios { get; set; }


        public DbSet<CartoonsPack> CartoonsPacks { get; set; }
        public DbSet<CartoonsPackLabelType> CartoonsPackLabelTypes { get; set; }
        public DbSet<PackCartoon> PackCartoons { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(typeof(CartoonsDatabaseContext).Assembly);
        }
    }
}
