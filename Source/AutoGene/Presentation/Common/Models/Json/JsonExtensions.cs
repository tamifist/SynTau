using System.Web;

namespace Presentation.Common.Models.Json
{
    /// <summary>
    /// Delegate for jsonizer
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public delegate IHtmlString Jsonizer(object obj);
}
