using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Common.Contracts.Entities;
using Data.Common.Services.Tests.TestData;
using Data.Ecommerce.Contracts.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Ecommerce.Services.Tests.RepositoryIntegrationTests
{
    public class EcommerceEntityCreator: CommonEntityCreator
    {
        public EcommerceEntityCreator(DbContext dbContext) : base(dbContext)
        {
        }

        public GeneOrder CreateGeneOrder()
        {
            GeneOrder geneOrder = new GeneOrder
            {
                Name = "Poly T",
                Sequence = "TTTTTTT",
                User = CreateTestUser()
            };

            return GetOrCreate(geneOrder, () => RemoveEntity(geneOrder));
        }
    }
}
