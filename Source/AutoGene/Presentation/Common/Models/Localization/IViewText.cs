using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Presentation.Common.Models.Localization
{
    public interface IViewText
    {
        string Get(string key, params object[] args);
    }
}
