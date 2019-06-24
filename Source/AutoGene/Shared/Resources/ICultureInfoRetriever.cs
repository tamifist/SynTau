using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Resources
{
    public interface ICultureInfoRetriever
    {
        CultureInfo GetCurrentCultureInfo();
    }
}
