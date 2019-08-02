using System;
using System.Collections.Generic;
using System.Text;
using Data.Common.Services.Mappings;
using Microsoft.EntityFrameworkCore;

namespace Data.Common.Services
{
    public class BaseDbContextOptions
    {
        public readonly DbContextOptions<DbContext> Options;
        public readonly BaseDbContextSeed DbContextSeed;
        public readonly IEnumerable<IEntityMap> Mappings;

        public BaseDbContextOptions(DbContextOptions<DbContext> options, BaseDbContextSeed dbContextSeed, IEnumerable<IEntityMap> mappings)
        {
            DbContextSeed = dbContextSeed;
            Options = options;
            Mappings = mappings;
        }
    }
}
