using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Shared.Helpers
{
    public class NoCopyAttribute : Attribute { }

    public static class ObjectCopyer
    {
        public static void CopyAllFrom<T>(this T target, T source, bool includeFields = false) where T : class
        {
            var type = typeof(T);
            foreach (var sourceProperty in type.GetProperties())
            {
                if (sourceProperty.GetCustomAttribute<NoCopyAttribute>() != null) continue;

                var targetProperty = type.GetProperty(sourceProperty.Name);
                targetProperty.SetValue(target, sourceProperty.GetValue(source, null), null);
            }

            if (includeFields)
                foreach (var sourceField in type.GetFields())
                {
                    if (sourceField.GetCustomAttribute<NoCopyAttribute>() != null) continue;

                    var targetField = type.GetField(sourceField.Name);
                    targetField.SetValue(target, sourceField.GetValue(source));
                }
        }
    }
}
