using System.IO;
using Newtonsoft.Json;
using YamlDotNet.Serialization;

namespace CroquetAustralia.Library.Extensions
{
    public static class Serialization
    {
        public static string ToJson(this object value)
        {
            return JsonConvert.SerializeObject(value, Formatting.None);
        }

        public static T ToObject<T>(this string value)
        {
            return JsonConvert.DeserializeObject<T>(value);
        }

        public static string ToYaml(this object value)
        {
            using (var sw = new StringWriter())
            {
                new Serializer().Serialize(sw, value);

                return sw.ToString();
            }
        }
    }
}