using Data.Ecommerce.Contracts.Entities;
using Xunit;

namespace Data.Ecommerce.Services.Tests.RepositoryIntegrationTests
{
    public class GeneOrderRepositoryTests : EcommerceRepositoryIntegrationTests<GeneOrder>
    {
        protected override GeneOrder CreateEntity()
        {
            return EntityCreator.CreateGeneOrder();
        }

        [Fact]
        public void InsertOrUpdate_AddGeneOrder_AddedSuccessfully()
        {
            GeneOrder geneOrder = EntityCreator.CreateGeneOrder();

            InsertOrUpdate(geneOrder);

            GeneOrder createdGeneOrder = UnitOfWork.GetById<GeneOrder>(CreatedEntity.Id);
            Assert.NotNull(createdGeneOrder);
            Assert.NotNull(createdGeneOrder.UserId);
            Assert.NotNull(createdGeneOrder.User);
        }
    }
}