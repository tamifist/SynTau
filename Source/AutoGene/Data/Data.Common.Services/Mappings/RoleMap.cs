using Data.Common.Contracts.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Common.Services.Mappings
{
    public class RoleMap : BaseEntityMap<Role>
    {
        protected override void InternalMap(EntityTypeBuilder<Role> entityBuilder)
        {
            base.InternalMap(entityBuilder);

            entityBuilder.ToTable("Roles");

            entityBuilder
                .HasIndex(x => x.Name)
                .IsUnique();
            entityBuilder.Property(x => x.Name)
                .HasMaxLength(32)
                .IsRequired();

            entityBuilder.Property(x => x.Description)
                .HasMaxLength(128);
        }
    }
}
