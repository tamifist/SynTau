using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Presentation.Common.Models.Json
{
    /// <summary>
    /// interface for encoding object to json
    /// </summary>
    public interface IJsonText
    {
        /// <summary>
        /// method which encoded object to IHtmlSting
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        IHtmlString Encode(object obj);
    }
}
