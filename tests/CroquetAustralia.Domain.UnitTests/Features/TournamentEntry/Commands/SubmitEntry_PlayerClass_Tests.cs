using System;
using System.Collections.Generic;
using CroquetAustralia.Domain.Core;
using CroquetAustralia.Domain.Features.TournamentEntry.Commands;
using CroquetAustralia.TestHelpers;
using FluentAssertions;
using Xunit;

namespace CroquetAustralia.Domain.UnitTests.Features.TournamentEntry.Commands
{
    public class SubmitEntry_PlayerClass_Tests
    {
        public class IsAgeEligible : SubmitEntryTests
        {
            private const string TournamentStarts = "24 Sep 2016 Australia/Melbourne";

            public static IEnumerable<object[]> TheoryData = new[]
            {
                new object[] {"1 Jan 1994", TournamentStarts, false},
                new object[] {"31 Dec 1994", TournamentStarts, false},
                new object[] {"1 Jan 1995", TournamentStarts, true},
                new object[] {"1 Jan 1996", TournamentStarts, true},
                new object[] {"1 Jan 1997", TournamentStarts, true},
                new object[] {"23 Sep 1998", TournamentStarts, true},
                new object[] {"24 Sep 1998", TournamentStarts, true},
                new object[] {"25 Sep 1998", TournamentStarts, true}
            };

            [Theory, MemberData(nameof(TheoryData))]
            public void Should_return_expected_value(string dateOfBirth, string tournamentStarts, bool expected)
            {
                // Given
                var player = Dummy.Value<SubmitEntry.PlayerClass>();
                var starts = tournamentStarts.ToZonedDateTime();

                player.SetProperty("DateOfBirth", DateTime.Parse(dateOfBirth));

                // When
                var isAgeEligible = player.IsAgeEligible(starts);

                // Then
                isAgeEligible.Should().Be(expected);
            }
        }

        public class IsUnder18AnytimeDuringTournament : SubmitEntryTests
        {
            private const string PracticeStarts = "23 Sep 2016 Australia/Melbourne";
            private const string TournamentStarts = "24 Sep 2016 Australia/Melbourne";

            public static IEnumerable<object[]> TheoryData = new[]
            {
                new object[] {"1 Jan 1994", PracticeStarts, TournamentStarts, false},
                new object[] {"31 Dec 1994", PracticeStarts, TournamentStarts, false},
                new object[] {"1 Jan 1995", PracticeStarts, TournamentStarts, false},
                new object[] {"1 Jan 1996", PracticeStarts, TournamentStarts, false},
                new object[] {"1 Jan 1997", PracticeStarts, TournamentStarts, false},
                new object[] {"23 Sep 1998", PracticeStarts, TournamentStarts, false},
                new object[] {"24 Sep 1998", PracticeStarts, TournamentStarts, true},
                new object[] {"25 Sep 1998", PracticeStarts, TournamentStarts, true},
                new object[] {"1 Jan 1999", PracticeStarts, TournamentStarts, true}
            };

            [Theory, MemberData(nameof(TheoryData))]
            public void Should_return_expected_value(string dateOfBirth, string tournamentPracticeStarts, string tournamentStarts, bool expected)
            {
                // Given
                var player = Dummy.Value<SubmitEntry.PlayerClass>();
                var practiceStarts = tournamentPracticeStarts.ToZonedDateTime();
                var starts = tournamentStarts.ToZonedDateTime();

                player.SetProperty("DateOfBirth", DateTime.Parse(dateOfBirth));

                // When
                var isUnder18AnytimeDuringTournamentOrPractice = player.IsUnder18AnytimeDuringTournamentOrPractice(practiceStarts, starts);

                // Then
                isUnder18AnytimeDuringTournamentOrPractice.Should().Be(expected);
            }
        }
    }
}