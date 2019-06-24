using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Resources
{
    public interface IResourceContainer
    {
        string GetString(string key);
    }
}
