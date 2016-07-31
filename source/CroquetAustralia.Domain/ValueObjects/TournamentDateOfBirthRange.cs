using System;
using NodaTime;

namespace CroquetAustralia.Domain.ValueObjects
{
    public class TournamentDateOfBirthRange
    {
        public TournamentDateOfBirthRange(ZonedDateTime starts)
        {
            MinimumValue = new DateTime(starts.Year - 21, 1, 1);
            MaximumValue = starts.ToDateTimeUnspecified();
            Under18 = MaximumValue.AddYears(-18).AddDays(1);
        }

        public DateTime MinimumValue { get; }
        public DateTime MaximumValue { get; }
        public DateTime Under18 { get; }
    }
}