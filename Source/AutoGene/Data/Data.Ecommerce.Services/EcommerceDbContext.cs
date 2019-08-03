using Data.Common.Services;
using Shared.Framework.Dependency;

namespace Data.Ecommerce.Services
{
    public class EcommerceDbContext : BaseDbContext, IScopedDependency
    {
        public EcommerceDbContext(IEcommerceDbContextOptionsFactory ecommerceDbContextOptionsFactory)
            : base(ecommerceDbContextOptionsFactory.Create())
        {
        }
    }
}