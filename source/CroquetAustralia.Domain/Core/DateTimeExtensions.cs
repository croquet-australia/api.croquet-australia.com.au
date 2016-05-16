using System;
using NodaTime;
using NodaTime.Text;

namespace CroquetAustralia.Domain.Core
{
    internal static class DateTimeExtensions
    {
        internal static ZonedDateTime ToZonedDateTime(this string text)
        {
            var dateTimeSecondsPattern = ZonedDateTimePattern.CreateWithInvariantCulture("dd MMM yyyy HH:mm:ss z", DateTimeZoneProviders.Tzdb);
            var dateTimeSecondsResult = dateTimeSecondsPattern.Parse(text);

            if (dateTimeSecondsResult.Success)
            {
                return dateTimeSecondsResult.Value;
            }

            var dateTimePattern = ZonedDateTimePattern.CreateWithInvariantCulture("dd MMM yyyy HH:mm z", DateTimeZoneProviders.Tzdb);
            var dateTimeResult = dateTimePattern.Parse(text);

            if (dateTimeResult.Success)
            {
                return dateTimeResult.Value;
            }

            var datePattern = ZonedDateTimePattern.CreateWithInvariantCulture("dd MMM yyyy z", DateTimeZoneProviders.Tzdb);
            var dateResult = datePattern.Parse(text);

            if (dateResult.Success)
            {
                return dateResult.Value;
            }

            throw new AggregateException(dateTimeResult.Exception, dateResult.Exception);
        }
    }
}