using Data.Contracts.Entities.GeneEditor;
using Data.Services.Tests.RepositoryIntegrationTests.Base;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Data.Services.Tests.RepositoryIntegrationTests
{
    [TestClass]
    public class AminoAcidRepositoryTests : RepositoryIntegrationTests<AminoAcid>
    {
        protected override AminoAcid CreateEntity()
        {
            return EntityCreator.CreateAminoAcid();
        }
    }
}