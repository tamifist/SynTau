using System;
using System.Collections.Generic;
using System.Text;
using Data.Common.Services;

namespace Data.Ecommerce.Services
{
    public interface IEcommerceDbContextOptionsFactory
    {
        BaseDbContextOptions Create();
    }
}
