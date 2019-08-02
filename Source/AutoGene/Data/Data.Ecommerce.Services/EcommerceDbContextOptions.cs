using System;
using System.Collections.Generic;
using System.Text;
using Data.Common.Services;
using Data.Common.Services.Mappings;
using Microsoft.EntityFrameworkCore;

namespace Data.Ecommerce.Services
{
    public class EcommerceDbContextOptions: BaseDbContextOptions
    {
        public EcommerceDbContextOptions(DbContextOptions<DbContext> options, BaseDbContextSeed dbContextSeed, IEnumerable<IEntityMap> mappings) 
            : base(options, dbContextSeed, mappings)
        {
        }
    }
}
