using System.Reflection;
using Data.Common.Contracts.Attributes;
using Data.Common.Contracts.Entities;
using Data.Common.Services;
using Data.Ecommerce.Contracts.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Data.Ecommerce.Services
{
    public class EcommerceDbContext : BaseDbContext
    {
        public EcommerceDbContext(DbContextOptions<DbContext> options)
            : base(options)
        {
        }

        public DbSet<GeneOrder> GeneOrders { get; set; }
    }
}