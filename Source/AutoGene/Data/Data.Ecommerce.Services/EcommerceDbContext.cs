using Data.Common.Services;

namespace Data.Ecommerce.Services
{
    public class EcommerceDbContext : BaseDbContext
    {
        public EcommerceDbContext(CommonDbContextOptions options)
            : base(options)
        {
        }
    }
}