using Data.Contracts.Entities.CycleEditor;
using Data.Contracts.Entities.OligoSynthesizer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Data.Services.Tests.RepositoryIntegrationTests.Base;

namespace Data.Services.Tests.RepositoryIntegrationTests
{
    [TestClass]
    public class OligoSynthesisActivityRepositoryTests : RepositoryIntegrationTests<OligoSynthesisActivity>
    {
        protected override OligoSynthesisActivity CreateEntity()
        {
            return EntityCreator.CreateOligoSynthesisActivity();
        }
    }
}