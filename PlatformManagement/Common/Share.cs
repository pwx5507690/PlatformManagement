using System;
using System.Collections.Generic;
using WebGrease.Css.Extensions;

namespace PlatformManagement.Common
{
    public static class Share
    {
        public static string ToCamelCase(this string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return value;
            }
            var firstChar = value[0];
            if (char.IsLower(firstChar))
            {
                return value;
            }
            firstChar = char.ToLowerInvariant(firstChar);
            return string.Format("{0}{1}", firstChar, value.Substring(1));
        }

        public static IEnumerable<T> ForIn<T>(this IEnumerable<T> queues,Action<T> action)
        {
           queues.ForEach(t =>
            {
                action(t);
            }); 
            return queues;
        }
        
    }
}