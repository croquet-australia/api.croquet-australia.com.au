using System;
using NodaTime;

namespace CroquetAustralia.Domain.Features.TournamentEntry.Commands
{
    public static class SubmitEntry_PlayerClass_Extensions
    {
        public static bool IsUnder18AnytimeDuringTournament(this SubmitEntry.PlayerClass player, ZonedDateTime tournamentStarts)
        {
            if (!player.DateOfBirth.HasValue)
            {
                throw new NotSupportedException($"{nameof(IsUnder18AnytimeDuringTournament)} is not supported when DateOfBirth is null.");
            }
            return player.DateOfBirth > tournamentStarts.ToDateTimeUnspecified().AddYears(-18);
        }

        public static bool IsAgeEligible(this SubmitEntry.PlayerClass player, ZonedDateTime tournamentStarts)
        {
            if (!player.DateOfBirth.HasValue)
            {
                throw new NotSupportedException($"{nameof(IsAgeEligible)} is not supported when DateOfBirth is null.");
            }

            return player.DateOfBirth < tournamentStarts.ToDateTimeUnspecified() && player.DateOfBirth >= new DateTime(tournamentStarts.Year - 21, 1, 1);
        }
    }
}