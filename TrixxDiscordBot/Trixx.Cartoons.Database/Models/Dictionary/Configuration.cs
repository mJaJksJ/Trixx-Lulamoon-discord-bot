using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Trixx.Database.Common.Utils;

namespace Trixx.Cartoons.Database.Models.Dictionary
{
    public sealed class Configuration :
        IEntityTypeConfiguration<DictionaryCartoon>,
        IEntityTypeConfiguration<DictionaryStudio>,
        IEntityTypeConfiguration<DictionaryCatroonStudio>
    {
        public void Configure(EntityTypeBuilder<DictionaryCartoon> builder)
        {
            builder.Property(x => x.AlternativeNames).Json();
            builder.Property(x => x.Sources).Json();

            builder.HasOne(x => x.SystemObject)
                .WithOne(x => x.DictionaryCartoon)
                .HasForeignKey<CartoonSystemObject>(c => c.DictionaryCartoonId);
        }

        public void Configure(EntityTypeBuilder<DictionaryStudio> builder)
        {
            builder.HasOne(x => x.SystemObject)
                .WithOne(x => x.DictionaryStudio)
                .HasForeignKey<CartoonSystemObject>(c => c.DictionaryStudioId);
        }

        public void Configure(EntityTypeBuilder<DictionaryCatroonStudio> builder)
        {
            builder.HasIndex(x => new { x.CartoonId, x.DictionaryStudioId }).IsUnique(true);
        }
    }
}
