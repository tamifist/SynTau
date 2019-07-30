using System.Collections.Generic;
using Data.Common.Services.Mappings;
using Microsoft.EntityFrameworkCore;
using Shared.Resources;

namespace Data.Common.Services
{
    public static class BaseDbContextFactory
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
            }));

            return dbContext;
        }
    }
}
