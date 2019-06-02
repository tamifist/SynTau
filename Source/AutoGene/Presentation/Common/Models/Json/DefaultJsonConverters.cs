using System.Collections.Generic;
using Newtonsoft.Json;

namespace Presentation.Common.Models.Json
{
    /// <summary>
    /// Null Json Converters.
    /// </summary>
    public static class DefaultJsonConverters
    {
        private static readonly IEnumerable<JsonConverter> instance;

        static DefaultJsonConverters()
        {
            instance = new List<JsonConverter>();// { new JsonDateTimeConrverter() };
        }

        /// <summary>
        /// The instance of <see cref="DefaultJsonConverters"/> class.
        /// </summary>
        public static IEnumerable<JsonConverter> Instance
        {
            get
            {
                return instance;
            }
        }
    }
}