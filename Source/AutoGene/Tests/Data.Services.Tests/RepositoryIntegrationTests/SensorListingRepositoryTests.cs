using Data.Contracts.Entities.SystemMonitor;
using Data.Services.Tests.RepositoryIntegrationTests.Base;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Data.Services.Tests.RepositoryIntegrationTests
{
    [TestClass]
    public class SensorListingRepositoryTests : RepositoryIntegrationTests<SensorListing>
    {
        protected override SensorListing CreateEntity()
        {
            return EntityCreator.CreateSensorListing();
        }
    }
}