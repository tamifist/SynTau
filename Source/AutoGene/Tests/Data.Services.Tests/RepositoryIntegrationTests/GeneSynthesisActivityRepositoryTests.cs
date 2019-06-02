using Data.Contracts.Entities.GeneSynthesizer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Data.Services.Tests.RepositoryIntegrationTests.Base;

namespace Data.Services.Tests.RepositoryIntegrationTests
{
    [TestClass]
    public class GeneSynthesisActivityRepositoryTests : RepositoryIntegrationTests<GeneSynthesisActivity>
    {
        protected override GeneSynthesisActivity CreateEntity()
        {
            return EntityCreator.CreateGeneSynthesisActivity();
        }
    }
}