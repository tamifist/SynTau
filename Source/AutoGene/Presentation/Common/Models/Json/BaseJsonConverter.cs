using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Presentation.Common.Models.Json
{
    /// <summary>
    /// Base implementation of <see cref="JsonConverter"/> that restricts unnecessary read operation. Used by Jsonizer.
    /// </summary>
    public abstract class BaseJsonConverter : JsonConverter
    {
        public override bool CanRead
        {
            get
            {
                return false;
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException("Unnecessary because CanRead is false. The type will skip the converter.");
        }
    }
}
