using System;
using Newtonsoft.Json;

namespace Presentation.Common.Json
{
    /// <summary>
    /// Converts float or double value to json representation with comma as decimal separator.
    /// </summary>
    public class JsonFloatConverter : BaseJsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(float) || objectType == typeof(float?);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var valueResult = value.ToString().Replace(".", ",");
            writer.WriteValue(valueResult);
            writer.Flush();
        }
    }
}