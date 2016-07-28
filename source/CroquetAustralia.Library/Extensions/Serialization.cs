using Newtonsoft.Json;

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
    }
}