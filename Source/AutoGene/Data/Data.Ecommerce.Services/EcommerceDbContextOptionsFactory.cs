using System.Collections.Generic;
using Data.Common.Services;
using Data.Common.Services.Mappings;
using Data.Ecommerce.Services.Mappings;
using Microsoft.EntityFrameworkCore;
using Shared.Framework.Dependency;
using Shared.Resources;

namespace Data.Ecommerce.Services
{
    public class EcommerceDbContextOptionsFactory: IEcommerceDbContextOptionsFactory, ISingletonDependency
    {
        public BaseDbContextOptions Create()
        {
            var optionsBuilder = new DbContextOptionsBuilder<DbContext>();
            optionsBuilder.UseSqlServer(
                Configuration.Environment.DbConnection, 
                x => x.MigrationsAssembly(Configuration.MigrationsAssemblyName));

            return new BaseDbContextOptions(optionsBuilder.Options, new BaseDbContextSeed(), new List<IEntityMap>()
            {
                new UserMap(),
                new RoleMap(),
                new UserRoleMap(),
                new GeneOrderMap()
            });
        }
    }
}
