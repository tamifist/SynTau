using System.Collections.Generic;
using Data.Common.Services;
using Data.Common.Services.Mappings;
using Data.Ecommerce.Services.Mappings;
using Microsoft.EntityFrameworkCore;
using Shared.Resources;

namespace Data.Ecommerce.Services
{
    public static class EcommerceDbContextFactory
    {
        public static BaseDbContext Create()
        {
            var optionsBuilder = new DbContextOptionsBuilder<DbContext>();
            optionsBuilder.UseSqlServer(Configuration.Environment.DbConnection);
            var dbContext = new BaseDbContext(new CommonDbContextOptions(optionsBuilder.Options, new BaseDbContextSeed(), new List<IEntityMap>()
            {
                new UserMap(),
                new RoleMap(),
                new UserRoleMap(),
                new GeneOrderMap()
            }));

            return dbContext;
        }
    }
}
