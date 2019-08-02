using Data.Common.Services;

namespace Data.Ecommerce.Services
{
    public class EcommerceDbContext : BaseDbContext
    {
        public EcommerceDbContext(BaseDbContextOptions options)
            : base(options)
        {
        }
    }
}