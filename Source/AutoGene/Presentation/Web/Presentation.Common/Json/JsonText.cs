using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Html;
using Newtonsoft.Json;
using Shared.Framework.Dependency;

namespace Presentation.Common.Json
{
    /// <summary>
    /// Encode object to json.
    /// </summary>
    public class JsonText : IJsonText, IScopedDependency
    {
        private readonly IEnumerable<JsonConverter> jsonConverters;

        public JsonText(IEnumerable<JsonConverter> converters)
        {
            jsonConverters = converters;
        }

        /// <summary>
        /// Newtonsoft json serializer 
        /// </summary>
        /// <returns></returns>
        public IHtmlContent Encode(object obj)
        {
            string serializedObject = JsonConvert.SerializeObject(obj, jsonConverters.ToArray());
            return new HtmlString(serializedObject);
        }
    }
}
