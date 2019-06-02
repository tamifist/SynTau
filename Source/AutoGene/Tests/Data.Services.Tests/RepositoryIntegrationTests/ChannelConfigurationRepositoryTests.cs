using Data.Contracts.Entities.GeneEditor;
using Data.Contracts.Entities.SystemConfiguration;
using Data.Services.Tests.RepositoryIntegrationTests.Base;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Data.Services.Tests.RepositoryIntegrationTests
{
    [TestClass]
    public class ChannelConfigurationRepositoryTests : RepositoryIntegrationTests<ChannelConfiguration>
    {
        protected override ChannelConfiguration CreateEntity()
        {
            return EntityCreator.CreateChannelConfiguration();
        }
    }
}