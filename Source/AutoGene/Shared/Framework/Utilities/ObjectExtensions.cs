using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Autofac;

namespace Shared.Framework.Utilities
{
    public static class ObjectExtensions
    {
        /// <summary>
        /// Returns a string with all properties of the object dumped.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static string DumpToString(this object entity)
        {
            var result = new StringBuilder();
            result.Append(entity.GetType().Name);
            result.Append("{");
            WriteObjectToBuilder(entity, result);
            result.Append("}");
            return result.ToString();
        }

        private static void WriteObjectToBuilder(object entity, StringBuilder result)
        {
            Dictionary<string, string> properties = entity.GetType().GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                                                          .Where(item => item.CanRead)
                                                          .ToDictionary(item => item.Name, item => GetPropertyValue(entity, item));

            foreach (KeyValuePair<string, string> property in properties)
            {
                result.AppendFormat("{0} = {1},", property.Key, property.Value);
            }

            result.Remove(result.Length - 1, 1);
        }

        private static string GetPropertyValue(object @object, PropertyInfo item)
        {
            if (item.PropertyType.IsPrimitive ||
                item.PropertyType.IsAssignableTo<String>() | item.PropertyType.IsAssignableTo<ValueType>())
            {
                return Convert.ToString(item.GetValue(@object, null));
            }
            else
            {
                return "**Type: " + item.PropertyType.Name;
            }
        }
    }
}