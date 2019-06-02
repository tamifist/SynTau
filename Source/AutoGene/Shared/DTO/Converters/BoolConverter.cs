using System;
using Newtonsoft.Json;

namespace Shared.DTO.Converters
{
    /// <summary>
    /// This method used to not generate converting error when we get data from local storage 
    /// </summary>
    public class BoolConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(value);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return reader.Value ?? false;
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(bool);
        }
    }
}