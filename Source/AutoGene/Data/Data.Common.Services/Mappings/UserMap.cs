using Data.Common.Contracts.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Common.Services.Mappings
{
    public class UserMap : BaseEntityMap<User>
    {
        protected override void InternalMap(EntityTypeBuilder<User> entityBuilder)
        {
            base.InternalMap(entityBuilder);

            entityBuilder.ToTable("Users");

            entityBuilder
                .HasIndex(x => x.Email)
                .IsUnique();
            entityBuilder.Property(x => x.Email)
                .HasMaxLength(255)
                .IsRequired();

            entityBuilder.Property(x => x.Password)
                .HasMaxLength(255)
                .IsRequired();

            entityBuilder.Property(x => x.PasswordSalt)
                .IsRequired();

            entityBuilder.Property(x => x.FirstName)
                .HasMaxLength(50)
                .IsRequired();

            entityBuilder.Property(x => x.LastName)
                .HasMaxLength(50)
                .IsRequired();

            entityBuilder.Property(x => x.Organization)
                .HasMaxLength(255);

            entityBuilder.Property(x => x.LabGroup)
                .HasMaxLength(255);
        }
    }
}
