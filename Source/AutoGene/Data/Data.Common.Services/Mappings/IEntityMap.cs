using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Data.Common.Services.Mappings
{
    public interface IEntityMap
    {
        void Map(ModelBuilder builder);
    }
}
