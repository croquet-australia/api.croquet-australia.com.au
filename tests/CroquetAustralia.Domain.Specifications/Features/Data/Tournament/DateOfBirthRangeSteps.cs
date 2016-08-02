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

        [Given(@"tournament practiceStarts '(.*)'")]
        public void GivenTournamentPracticeStarts(string practiceStarts)
        {
            Given.DummyTournament.PracticeStarts = practiceStarts.ToZonedDateTime();
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

        [When(@"I get DateOfBirthRange")]
        public void WhenIGetDateOfBirthRange()
        {
            Given.Tournament = Given.DummyTournament.Build();

            Actual.Invoke(() => Actual.DateOfBirthRange = Given.Tournament.DateOfBirthRange);
        }

        [Then(@"the result should not be null")]
        public void ThenTheResultShouldNotBeNull()
        {
            Actual.DateOfBirthRange.Should().NotBeNull();
        }

        [Then(@"result\.Minimum should be '(.*)'")]
        public void ThenResult_MinimumValueShouldBe(string expected)
        {
            Actual.DateOfBirthRange.Minimum.Should().Be(DateTime.Parse(expected));
        }

        [Then(@"result\.Maximum should be '(.*)'")]
        public void ThenResult_MaximumValueShouldBe(string expected)
        {
            Actual.DateOfBirthRange.Maximum.Should().Be(DateTime.Parse(expected));
        }

        [Then(@"the result should be null")]
        public void ThenTheResultShouldBeNull()
        {
            Actual.DateOfBirthRange.Should().BeNull();
        }
    }
}