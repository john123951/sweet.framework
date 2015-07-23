using System;

namespace sweet.framework.Utility.Extention
{
    public static class ReflectionExtensions
    {
        public static bool IsNullable(this Type type)
        {
            return type.IsGenericType && (typeof(Nullable<>) == type.GetGenericTypeDefinition());
        }

        public static bool IsConcrete(this Type type)
        {
            return (!type.IsInterface && !type.IsAbstract && !type.IsValueType);
        }
    }
}