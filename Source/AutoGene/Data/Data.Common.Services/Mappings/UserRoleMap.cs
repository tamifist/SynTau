using Data.Common.Contracts.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Common.Services.Mappings
{
    public class UserRoleMap : IEntityMap
    {
        public void Map(ModelBuilder modelBuilder)
        {
            EntityTypeBuilder<UserRole> entityBuilder = modelBuilder.Entity<UserRole>();

            entityBuilder.ToTable("RoleUsers");

            entityBuilder
                .HasKey(x => new { x.UserId, x.RoleId });

            entityBuilder.Property(x => x.UserId).HasColumnName("User_Id");

            entityBuilder.Property(x => x.RoleId).HasColumnName("Role_Id");
        }
    }
}
