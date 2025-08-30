using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Trixx.Database.Common.Utils;
using Trixx.Database.Models.Identity;

namespace Trixx.Database.Models.FormDefaults
{
    public class Configuration :
        IEntityTypeConfiguration<FormDeafault>
    {
        public void Configure(EntityTypeBuilder<FormDeafault> builder)
        {
            builder.HasKey(x => x.Type);
            builder.Property(x => x.Cartoon).Json();
        }
    }
}
