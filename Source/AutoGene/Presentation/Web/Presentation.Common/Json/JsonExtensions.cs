using Microsoft.AspNetCore.Html;

namespace Presentation.Common.Json
{
    /// <summary>
    /// Delegate for jsonizer
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public delegate IHtmlContent Jsonizer(object obj);
}
