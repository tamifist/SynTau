using System.Collections.Generic;
using Data.Common.Services.Mappings;
using Microsoft.EntityFrameworkCore;
using Shared.Resources;

namespace Data.Common.Services
{
    public static class BaseDbContextOptionsFactory
    {
        public static BaseDbContextOptions Create()
        {
            var optionsBuilder = new DbContextOptionsBuilder<DbContext>();
            optionsBuilder.UseSqlServer(Configuration.Environment.DbConnection);

            return new BaseDbContextOptions(optionsBuilder.Options, new BaseDbContextSeed(), new List<IEntityMap>()
            {
                new UserMap(),
                new RoleMap(),
                new UserRoleMap(),
            });
        }
    }
}
