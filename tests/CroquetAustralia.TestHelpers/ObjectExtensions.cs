using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Anotar.NLog;
using YamlDotNet.Serialization;

namespace CroquetAustralia.TestHelpers
{
    public static class ObjectExtensions
    {
        public static string ToYaml(this object value)
        {
            return value.ToYaml(Enumerable.Empty<IYamlTypeConverter>());
        }

        public static string ToYaml(this object value, IEnumerable<IYamlTypeConverter> typeConverters)
        {
            LogTo.Trace($"{nameof(ToYaml)}(object {value})");

            var serializer = CreateSerializer(typeConverters);

            using (var sw = new StringWriter())
            {
                Serialize(value, sw, serializer);

                return sw.ToString();
            }
        }

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

        private static void Serialize(object value, StringWriter sw, Serializer serializer)
        {
            LogTo.Trace($"{nameof(Serialize)}(object {value}, {nameof(StringWriter)})");

            try
            {
                serializer.Serialize(sw, value);
            }
            catch (Exception exception)
            {
                var message = $"Cannot serialize '{value}' as YAML.";
                LogTo.ErrorException(message, exception);
                throw new Exception(message, exception);
            }
        }

        private static Serializer CreateSerializer(IEnumerable<IYamlTypeConverter> typeConverters)
        {
            return CreateSerializer(typeConverters.ToArray());
        }

        private static Serializer CreateSerializer(IYamlTypeConverter[] typeConverters)
        {
            var serializer = new Serializer(SerializationOptions.DisableAliases);

            foreach (var yamlTypeConverter in typeConverters)
            {
                serializer.RegisterTypeConverter(yamlTypeConverter);
            }

            return serializer;
        }
    }
}