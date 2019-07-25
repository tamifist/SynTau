﻿using Data.Common.Contracts.Entities;
using Data.Common.Services.Tests.RepositoryIntegrationTests.Base;
using Microsoft.EntityFrameworkCore;
using System.Configuration;
using Data.Common.Services.Tests.TestData;

namespace Data.Ecommerce.Services.Tests.RepositoryIntegrationTests
{
    public abstract class EcommerceRepositoryIntegrationTests<T> : 
        RepositoryIntegrationTestsBase<T, EcommerceEntityCreator> where T : BaseEntity, new()
    {
        protected override DbContext CreateDbContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<DbContext>();
            optionsBuilder.UseSqlServer(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            var dbContext = new EcommerceDbContext(optionsBuilder.Options);

            return dbContext;
        }

        protected override EcommerceEntityCreator CreateEntityCreator(DbContext context)
        {
            return new EcommerceEntityCreator(context);
        }
    }
}