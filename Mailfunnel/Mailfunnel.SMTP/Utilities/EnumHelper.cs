using System;
using System.ComponentModel;
using System.Reflection;

namespace Mailfunnel.SMTP.Utilities
{
    public static class EnumHelper
    {
        public static bool TryGetEnumByDescriptionAttribute<T>(string value, out T result)
        {
            var enumType = typeof(T);
            foreach (T val in Enum.GetValues(enumType))
            {
                var fi = enumType.GetField(val.ToString());
                var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attributes.Length == 0) continue;

                var attr = attributes[0];
                if (attr.Description == value)
                {
                    result = val;
                    return true;
                }
            }

            result = (T)(object)0;
            return false;
        }
    }
}
