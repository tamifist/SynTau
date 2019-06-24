using System.Collections.Generic;
using System.Linq;

namespace Shared.Enum.Attributes
{
    public static class EnumUtil
    {
        public static IEnumerable<T> GetValues<T>()
        {
            return System.Enum.GetValues(typeof(T)).Cast<T>();
        }
    }
}
