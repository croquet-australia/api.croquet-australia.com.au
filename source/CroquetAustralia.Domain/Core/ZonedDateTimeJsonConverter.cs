using System;
using System.Linq;
using Newtonsoft.Json;
using NodaTime;
using NodaTime.Text;

namespace CroquetAustralia.Domain.Core
{
    public class ZonedDateTimeJsonConverter : JsonConverter
    {
        private readonly ZonedDateTimePattern _dateTimePattern;
        private readonly Type[] _convertableTypes;

        public ZonedDateTimeJsonConverter(IDateTimeZoneProvider provider)
        {
            _convertableTypes = new [] {typeof(ZonedDateTime), typeof(ZonedDateTime?)};
            _dateTimePattern = ZonedDateTimePattern.CreateWithInvariantCulture("yyyy'-'MM'-'dd'T'HH':'mm':'ss", provider);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var writeValue = GetWriteValue(value);

            serializer.Serialize(writer, writeValue);
        }

        private object GetWriteValue(object value)
        {
            if (value == null)
            {
                return null;
            }

            var zonedDateTime = (ZonedDateTime)value;

            var writeValue = new
            {
                dateTime = _dateTimePattern.Format(zonedDateTime),
                timeZone = zonedDateTime.Zone.Id
            };
            return writeValue;
        }

        public override bool CanConvert(Type objectType)
        {
            return _convertableTypes.Contains(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}