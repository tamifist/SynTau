using Data.Contracts.Entities.GeneEditor;
using Data.Services.Tests.RepositoryIntegrationTests.Base;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Data.Services.Tests.RepositoryIntegrationTests
{
    [TestClass]
    public class OrganismRepositoryTests : RepositoryIntegrationTests<Organism>
    {
        protected override Organism CreateEntity()
        {
            return EntityCreator.CreateOrganism();
        }
    }
}