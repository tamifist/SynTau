using Data.Common.Contracts.Entities;
using Data.Ecommerce.Contracts.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Data.Ecommerce.Services.Tests.RepositoryIntegrationTests
{
    [TestClass]
    public class GeneOrderRepositoryTests : EcommerceRepositoryIntegrationTests<GeneOrder>
    {
        protected override GeneOrder CreateEntity()
        {
            return EntityCreator.CreateGeneOrder();
        }

        [TestMethod]
        public void InsertOrUpdate_AddGeneOrder_AddedSuccessfully()
        {
            GeneOrder geneOrder = EntityCreator.CreateGeneOrder();
            InsertOrUpdate(geneOrder);
            GeneOrder x = UnitOfWork.GetById<GeneOrder>(CreatedEntity.Id);
            Assert.IsNotNull(x);
        }
    }
}