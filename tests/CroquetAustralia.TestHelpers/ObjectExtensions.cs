using System;
using System.Linq;
using System.Reflection;

namespace CroquetAustralia.TestHelpers
{
    public static class ObjectExtensions
    {
        public static T SetProperty<T>(this T obj, string propertyName, object propertyValue)
        {
            var propertyNames = propertyName.Split('.');
            var parentObject = (object)obj;

            for (var parentPropertyIndex = 0; parentPropertyIndex < propertyNames.Length - 1; parentPropertyIndex++)
            {
                var parentPropertyName = propertyNames[parentPropertyIndex];
                parentObject = GetPropertyInfo(parentPropertyName, parentObject).GetValue(parentObject);
            }

            var propertyInfo = GetPropertyInfo(propertyNames.Last(), parentObject);

            propertyInfo.SetValue(parentObject, propertyValue);

            return obj;
        }

        private static PropertyInfo GetPropertyInfo(string propertyName, object obj)
        {
            var propertyInfo = obj.GetType().GetProperty(propertyName);

            if (propertyInfo == null)
            {
                throw new Exception($"Cannot find {obj.GetType()}.{propertyName.Last()}.");
            }

            return propertyInfo;
        }
    }
}