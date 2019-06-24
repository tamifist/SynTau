using System;
using System.Linq;
using System.Reflection;

namespace Shared.Enum.Attributes
{
    public static class EnumExtensions
    {
        public static string GetDescription(this System.Enum enumVal)
        {
            EnumDescriptionAttribute descriptionAttribute = GetAttributeOfType<EnumDescriptionAttribute>(enumVal);
            if (descriptionAttribute != null)
            {
                return descriptionAttribute.Description;
            }

            return string.Empty;
        }

        private static T GetAttributeOfType<T>(System.Enum enumVal) where T : Attribute
        {
            var typeInfo = enumVal.GetType().GetTypeInfo();
            MemberInfo memberInfo = typeInfo.DeclaredMembers.FirstOrDefault(x => x.Name == enumVal.ToString());
            if (memberInfo != null)
            {
                T attribute = memberInfo.GetCustomAttribute<T>();
                if (attribute != null)
                {
                    return attribute;
                }
            }

            return null;
        }
    }
}