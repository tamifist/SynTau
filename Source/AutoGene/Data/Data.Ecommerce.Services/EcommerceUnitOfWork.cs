using System;
using System.Collections.Generic;
using System.Text;
using Data.Common.Services;
using Data.Common.Services.Helpers;
using Data.Ecommerce.Contracts;
using Microsoft.EntityFrameworkCore;
using Shared.Framework.Dependency;

namespace Data.Ecommerce.Services
{
    public class EcommerceUnitOfWork: UnitOfWork, IEcommerceUnitOfWork, IScopedDependency
    {
        public EcommerceUnitOfWork(EcommerceDbContext dbContext, IRepositoryFactory repositoryFactory) 
            : base(dbContext, repositoryFactory)
        {
        }
    }
}
