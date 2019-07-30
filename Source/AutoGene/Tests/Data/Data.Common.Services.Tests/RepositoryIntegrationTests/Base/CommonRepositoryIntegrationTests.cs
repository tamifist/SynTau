using System.Configuration;
using Data.Common.Contracts.Entities;
using Data.Common.Services.Tests.TestData;
using Microsoft.EntityFrameworkCore;
using Shared.Resources;

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
            return BaseDbContextFactory.Create();
        }
    }
}