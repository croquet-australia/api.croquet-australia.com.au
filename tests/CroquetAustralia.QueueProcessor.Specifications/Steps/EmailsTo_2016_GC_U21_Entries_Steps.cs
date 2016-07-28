using System.Linq;
using ApprovalTests.Reporters;
using CroquetAustralia.QueueProcessor.Specifications.TestHelpers;
using FluentAssertions;
using TechTalk.SpecFlow;

namespace CroquetAustralia.QueueProcessor.Specifications.Steps
{
    [Binding]
    [UseReporter(typeof(DiffReporter))]
    public class EmailsTo_2016_GC_U21_Entries_Steps : TestBase
    {
        private readonly ActualData _actual;
        private readonly GivenData _given;

        public EmailsTo_2016_GC_U21_Entries_Steps(GivenData given, ActualData actual)
        {
            _given = given;
            _actual = actual;
        }

        [Given(@"yearOfBirth is '(.*)'")]
        public void GivenYearOfBirthIs(int yearOfBirth)
        {
            _given.YearOfBirth = yearOfBirth;
        }

        [Given(@"nonResident is '(.*)'")]
        public void GivenNonResidentIs(bool nonResident)
        {
            _given.NonResident = nonResident;
        }

        [Then(@"the player should not be sent an email")]
        public void ThenThePlayerShouldNotBeSentAnEmail()
        {
            _actual.TournamentEntryEmails.Length.Should().Be(0);
        }

        [Then(@"the email should have attachment '(.*)'")]
        public void ThenTheEmailShouldHaveAttachment(string attachmentFileName)
        {
            var actualEmail = _actual.TournamentEntryEmails.Single();
            var actualAttachments = actualEmail.Attachments.ToArray();

            if (string.IsNullOrWhiteSpace(attachmentFileName))
            {
                actualAttachments.Length.Should().Be(0);
            }
            else
            {
                actualAttachments.Length.Should().Be(1);
                actualAttachments[0].Name.Should().Be($"U21Tournament.{attachmentFileName}");
            }
        }
    }
}