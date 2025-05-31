using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Trixx.Database.Enums;
using Trixx.Database.Utils;

namespace Trixx.Database.Models.Identity
{
    public sealed class Configuration :
        IEntityTypeConfiguration<TrixxUser>,
        IEntityTypeConfiguration<TrixxRole>,
        IEntityTypeConfiguration<TrixxUserRole>
    {
        public void Configure(EntityTypeBuilder<TrixxUser> builder)
        {
            var admin = new TrixxUser
            {
                Id = 1,
                Email = "trixx",
                NormalizedEmail = "TRIXX",
                SecurityStamp = "43142260-d8fe-47f3-abbe-40cdcf406f97",
                UserName = "trixx",
                FullName = "Trixx",
                NormalizedUserName = "TRIXX",
                PasswordHash = new PasswordHasher<TrixxUser>().HashPassword(null, "Admin1!"),
                ConcurrencyStamp = "1708d2d7-7f63-41ae-ab12-08fdce3e53e0",
                EmailConfirmed = false,
                LockoutEnabled = false,
            };
            builder.HasData(admin);

            builder.Property(x => x.Id).HasIdentityOptions(startValue: 100);
        }

        public void Configure(EntityTypeBuilder<TrixxRole> builder)
        {
            builder.HasData(new TrixxRole
            {
                Id = TrixxRole.ID_ADMIN,
                Name = "Администратор",
                NormalizedName = "АДМИНИСТРАТОР",
                IsReadOnly = true,
                Permissions = PermissionsMappings.Lines.Select(x => x.Permission).ToList(),
                ConcurrencyStamp = "c5b18af4-d272-4ee8-ac25-0009b9097533",
            });

            builder.Property(x => x.Id).HasIdentityOptions(startValue: 100);
            builder.Property(x => x.Permissions).Json();
        }

        public void Configure(EntityTypeBuilder<TrixxUserRole> builder)
        {
            builder.HasData(new TrixxUserRole
            {
                UserId = 1,
                RoleId = TrixxRole.ID_ADMIN
            });

            builder.HasOne(x => x.Role).WithMany().HasForeignKey(x => x.RoleId);
            builder.HasOne(x => x.User).WithMany(x => x.Roles).HasForeignKey(x => x.UserId);
            builder.HasIndex(x => new { x.UserId, x.RoleId });
        }
    }
}
