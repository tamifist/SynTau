using Data.Contracts.Entities.GeneEditor;
using Data.Contracts.Entities.GeneOrder;
using Data.Services.Tests.RepositoryIntegrationTests.Base;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Data.Services.Tests.RepositoryIntegrationTests
{
    [TestClass]
    public class GeneOrderRepositoryTests : RepositoryIntegrationTests<GeneOrder>
    {
        protected override GeneOrder CreateEntity()
        {
            return EntityCreator.CreateGeneOrder();
        }
    }
}