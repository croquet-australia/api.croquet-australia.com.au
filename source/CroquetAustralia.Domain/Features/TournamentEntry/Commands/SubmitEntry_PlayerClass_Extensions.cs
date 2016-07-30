using System;
using NodaTime;

namespace CroquetAustralia.Domain.Features.TournamentEntry.Commands
{
    public static class SubmitEntry_PlayerClass_Extensions
    {
        public static bool IsUnder18(this SubmitEntry.PlayerClass player, ZonedDateTime tournamentStarts)
        {
            if (!player.DateOfBirth.HasValue)
            {
                throw new NotSupportedException($"{nameof(IsUnder18)} is not supported when DateOfBirth is null.");
            }
            throw new NotImplementedException("Write spec");
        }

        public static bool IsAgeEligible(this SubmitEntry.PlayerClass player, ZonedDateTime tournamentStarts)
        {
            if (!player.DateOfBirth.HasValue)
            {
                throw new NotSupportedException($"{nameof(IsAgeEligible)} is not supported when DateOfBirth is null.");
            }

            return player.DateOfBirth < tournamentStarts.ToDateTimeUtc() && player.DateOfBirth >= new DateTime(tournamentStarts.Year - 21, 1, 1);
        }
    }
}