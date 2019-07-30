using Data.Common.Contracts.Entities;
using Data.Common.Services.Tests.RepositoryIntegrationTests.Base;
using Microsoft.EntityFrameworkCore;

namespace Data.Ecommerce.Services.Tests.RepositoryIntegrationTests
{
    public abstract class EcommerceRepositoryIntegrationTests<T> : 
        RepositoryIntegrationTestsBase<T, EcommerceEntityCreator> where T : BaseEntity, new()
    {
        protected override DbContext CreateDbContext()
        {
            return EcommerceDbContextFactory.Create();
        }

        protected override EcommerceEntityCreator CreateEntityCreator(DbContext context)
        {
            return new EcommerceEntityCreator(context);
        }
    }
}