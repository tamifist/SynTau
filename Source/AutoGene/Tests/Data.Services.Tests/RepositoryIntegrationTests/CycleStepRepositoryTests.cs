using Data.Contracts.Entities.CycleEditor;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Data.Services.Tests.RepositoryIntegrationTests.Base;

namespace Data.Services.Tests.RepositoryIntegrationTests
{
    [TestClass]
    public class CycleStepRepositoryTests : RepositoryIntegrationTests<CycleStep>
    {
        protected override CycleStep CreateEntity()
        {
            return EntityCreator.CreateCycleStep();
        }
    }
}