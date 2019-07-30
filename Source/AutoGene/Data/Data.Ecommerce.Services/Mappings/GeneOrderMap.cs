using Data.Common.Services.Mappings;
using Data.Ecommerce.Contracts.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Ecommerce.Services.Mappings
{
    public class GeneOrderMap : BaseEntityMap<GeneOrder>
    {
        protected override void InternalMap(EntityTypeBuilder<GeneOrder> entityBuilder)
        {
            base.InternalMap(entityBuilder);

            entityBuilder.ToTable("GeneOrders");

            entityBuilder.Property(x => x.Name)
                .IsRequired();

            entityBuilder.Property(x => x.Sequence)
                .IsRequired();

            
        }
    }
}
