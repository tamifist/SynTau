using Microsoft.AspNetCore.Html;

namespace Presentation.Common.Json
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
        IHtmlContent Encode(object obj);
    }
}
