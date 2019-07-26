﻿using System.Configuration;
using Data.Common.Contracts.Entities;
using Data.Common.Services.Tests.TestData;
using Microsoft.EntityFrameworkCore;

namespace Data.Common.Services.Tests.RepositoryIntegrationTests.Base
{
    public abstract class CommonRepositoryIntegrationTests<T> :
        RepositoryIntegrationTestsBase<T, CommonEntityCreator>
           where T : BaseEntity, new()
    {
        protected override CommonEntityCreator CreateEntityCreator(DbContext context)
        {
            return new CommonEntityCreator(context);
        }

        protected override DbContext CreateDbContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<DbContext>();
            optionsBuilder.UseSqlServer(Configuration.ConnectionStrings.DbConnection);
            var dbContext = new BaseDbContext(optionsBuilder.Options);

            return dbContext;
        }
    }
}