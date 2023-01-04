using System;
using System.Collections;

namespace APS.DotNetSDK.Utils
{
    public static class TypeExtensionMethods
    {
        public static bool IsSimpleObject(this Type type)
        {
            return type.IsPrimitive
              || type == typeof(string)
              || (Nullable.GetUnderlyingType(type) != null);
        }

        public static bool IsListObject(this Type type)
        {
            return typeof(IEnumerable).IsAssignableFrom(type);
        }
    }
}