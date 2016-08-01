using System;
using NodaTime;

namespace CroquetAustralia.Domain.ValueObjects
{
    public class TournamentDateOfBirthRange
    {
        public TournamentDateOfBirthRange(ZonedDateTime starts)
        {
            Minimum = new DateTime(starts.Year - 21, 1, 1);
            Maximum = starts.ToDateTimeUnspecified();
        }

        public DateTime Minimum { get; }
        public DateTime Maximum { get; }

        public bool IsValid(DateTime dateOfBirth)
        {
            return dateOfBirth >= Minimum && dateOfBirth <= Maximum;
        }
    }
}