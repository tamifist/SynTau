using System.Data.Entity;
using Data.Contracts.Entities;
using Data.Services.Tests.TestData;
using Microsoft.Azure.Mobile.Server;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Data.Services.Tests.RepositoryIntegrationTests.Base
{
    [TestClass]
    public abstract class RepositoryIntegrationTests<T> :
        RepositoryIntegrationTestsBase<T, TestDataCreator>
           where T : Entity, new()
    {
        protected override TestDataCreator CreateEntityCreator(EntityContext context)
        {
            return new TestDataCreator(context);
        }
    }
}