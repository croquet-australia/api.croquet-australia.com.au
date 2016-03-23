using System;
using Newtonsoft.Json;
using NodaTime;
using NodaTime.Text;

namespace CroquetAustralia.Domain.Core
{
    public class ZonedDateTimeJsonConverter : JsonConverter
    {
        private readonly ZonedDateTimePattern _dateTimePattern;
        private readonly Type _zonedDateTimeType;

        public ZonedDateTimeJsonConverter(IDateTimeZoneProvider provider)
        {
            _zonedDateTimeType = typeof(ZonedDateTime);
            _dateTimePattern = ZonedDateTimePattern.CreateWithInvariantCulture("yyyy'-'MM'-'dd'T'HH':'mm':'ss", provider);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var zonedDateTime = (ZonedDateTime)value;

            var writeValue = new
            {
                dateTime = _dateTimePattern.Format(zonedDateTime),
                timeZone = zonedDateTime.Zone.Id
            };

            serializer.Serialize(writer, writeValue);
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == _zonedDateTimeType;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}