using System;
using System.Collections.Generic;
using System.Text;
using Data.Common.Contracts.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Common.Services.Mappings
{
    public abstract class BaseEntityMap<TEntity> : IEntityMap
        where TEntity : BaseEntity
    {
        public void Map(ModelBuilder builder)
        {
            InternalMap(builder.Entity<TEntity>());
        }

        protected virtual void InternalMap(EntityTypeBuilder<TEntity> entityBuilder)
        {
            entityBuilder
                .HasKey(x => x.Id)
                .ForSqlServerIsClustered(false);

            entityBuilder
                .Property(x => x.Version)
                .IsRowVersion();

            entityBuilder
                .HasIndex(x => x.CreatedAt)
                .ForSqlServerIsClustered(true);

            entityBuilder
                .Property(x => x.CreatedAt)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("getdate()");

            entityBuilder
                .Property(x => x.UpdatedAt)
                .ValueGeneratedOnAddOrUpdate();
        }
    }
}
