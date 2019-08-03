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

            entityBuilder.ToTable("UserRoles");

            entityBuilder
                .HasKey(x => new { x.UserId, x.RoleId });
        }
    }
}
