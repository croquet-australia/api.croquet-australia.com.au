using System;
using CroquetAustralia.Domain.Core;
using CroquetAustralia.Domain.Specifications.TestHelpers;
using FluentAssertions;
using TechTalk.SpecFlow;

namespace CroquetAustralia.Domain.Specifications.Features.Data.Tournament
{
    [Binding]
    public class DateOfBirthRangeSteps : StepsBase
    {
        public DateOfBirthRangeSteps(Given given, Actual actual)
            : base(given, actual)
        {
        }

        [Given(@"tournament starts '(.*)'")]
        public void GivenTournamentStarts(string starts)
        {
            Given.DummyTournament.Starts = starts.ToZonedDateTime();
        }

        [Given(@"tournament finishes '(.*)'")]
        public void GivenTournamentFinishes(string finishes)
        {
            Given.DummyTournament.Finishes = finishes.ToZonedDateTime();
        }

        [Given(@"tournament is U21")]
        public void GivenTournamentIsU21()
        {
            Given.DummyTournament.IsUnder21 = true;
        }

        [Given(@"tournament is not U21")]
        public void GivenTournamentIsNotU21()
        {
            Given.DummyTournament.IsUnder21 = false;
        }

        [When(@"I get TournamentDateOfBirthRange")]
        public void WhenIGetTournamentDateOfBirthRange()
        {
            Given.Tournament = Given.DummyTournament.Build();

            Actual.Invoke(() => Actual.DateOfBirthRange = Given.Tournament.DateOfBirthRange);
        }

        [Then(@"the result should not be null")]
        public void ThenTheResultShouldNotBeNull()
        {
            Actual.DateOfBirthRange.Should().NotBeNull();
        }

        [Then(@"result\.MinimumValue should be '(.*)'")]
        public void ThenResult_MinimumValueShouldBe(string expected)
        {
            Actual.DateOfBirthRange.MinimumValue.Should().Be(DateTime.Parse(expected));
        }

        [Then(@"result\.MaximumValue should be '(.*)'")]
        public void ThenResult_MaximumValueShouldBe(string expected)
        {
            Actual.DateOfBirthRange.MaximumValue.Should().Be(DateTime.Parse(expected));
        }

        [Then(@"result\.Under18 should be '(.*)'")]
        public void ThenResult_UnderShouldBe(string expected)
        {
            Actual.DateOfBirthRange.Under18.Should().Be(DateTime.Parse(expected));
        }

        [Then(@"the result should be null")]
        public void ThenTheResultShouldBeNull()
        {
            Actual.DateOfBirthRange.Should().BeNull();
        }
    }
}