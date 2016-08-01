using System;
using CroquetAustralia.Domain.ValueObjects;
using NodaTime;

namespace CroquetAustralia.Domain.Features.TournamentEntry.Commands
{
    public static class SubmitEntry_PlayerClass_Extensions
    {
        public static bool IsUnder18AnytimeDuringTournamentOrPractice(this SubmitEntry.PlayerClass player, ZonedDateTime practiceStarts, ZonedDateTime tournamentStarts)
        {
            if (!player.DateOfBirth.HasValue)
            {
                throw new InvalidOperationException($"{nameof(IsUnder18AnytimeDuringTournamentOrPractice)} is not supported when DateOfBirth is null.");
            }

            var minimumDate = practiceStarts < tournamentStarts ? practiceStarts.ToDateTimeUnspecified() : tournamentStarts.ToDateTimeUnspecified();

            return player.DateOfBirth > minimumDate.AddYears(-18);
        }

        public static bool IsAgeEligible(this SubmitEntry.PlayerClass player, ZonedDateTime tournamentStarts)
        {
            if (!player.DateOfBirth.HasValue)
            {
                throw new InvalidOperationException($"{nameof(IsAgeEligible)} is not supported when DateOfBirth is null.");
            }

            return new TournamentDateOfBirthRange(tournamentStarts).IsValid(player.DateOfBirth.Value);
        }
    }
}