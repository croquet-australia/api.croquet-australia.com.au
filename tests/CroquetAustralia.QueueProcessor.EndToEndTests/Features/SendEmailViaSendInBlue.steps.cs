using System.IO;
using System.Linq;
using CroquetAustralia.Library.Extensions;
using CroquetAustralia.QueueProcessor.Email;
using CroquetAustralia.QueueProcessor.Email.EmailGenerators;
using CroquetAustralia.QueueProcessor.EndToEndTests.TestHelpers;
using FluentAssertions;
using TechTalk.SpecFlow;

namespace CroquetAustralia.QueueProcessor.EndToEndTests.Features
{
    [Binding]
    public class SendEmailViaSendInBlueSteps : StepsBase
    {
        public SendEmailViaSendInBlueSteps(Given given, Actual actual)
            : base(given, actual)
        {
        }

        [Given(@"a valid EmailMessage")]
        public void GivenAValidEmailMessage()
        {
            var attachments = new EmailAttachments(Enumerable.Empty<FileInfo>());

            GivenValidEmailMessageWithAttachments(attachments);
        }

        private void GivenValidEmailMessageWithAttachments(EmailAttachments attachments)
        {
            var to = new[] {new EmailAddress("tim@26tp.com", "Tim Murphy")};
            var cc = Enumerable.Empty<EmailAddress>();
            var bcc = Enumerable.Empty<EmailAddress>();
            var from = to.First();
            const string subject = "test via SendInBlue";
            const string bodyAsText = "some **text**";
            const string bodyAsHtml = "<p>some <b>text</b></p>";

            Given.EmailMessage = new EmailMessage(to, cc, bcc, from, subject, bodyAsText, bodyAsHtml, attachments);
        }

        [Given(@"a valid EmailMessage with 1 attachment")]
        public void GivenAValidEmailMessageWithOneAttachment()
        {
            var assembly = typeof(BaseEmailGenerator).Assembly;
            var attachment = new FileInfo(Path.Combine(Path.GetTempPath(), $"{nameof(SendEmailViaSendInBlueSteps)}.pdf"));

            assembly.SaveResourceAsFile("CroquetAustralia.QueueProcessor.Email.Templates.PayByCash.U21Tournament.Under 18 Consent Form NZ.pdf", attachment);

            GivenValidEmailMessageWithAttachments(new EmailAttachments(new[] {attachment}));
        }

        [When(@"EmailService\.SendAsync\(EmailMessage emailMessage\) is called")]
        public void WhenEmailService_SendAsyncEmailMessageEmailMessageIsCalled()
        {
            Actual.Invoke(() => Actual.MessageId = Given.EmailService.SendAsync(Given.EmailMessage).Result);
        }

        [Then(@"the email is sent")]
        public void ThenTheEmailIsSent()
        {
            Actual.Exception.Should().BeNull();
        }

        [Then(@"messageId is returned")]
        public void ThenMessageIdIsReturned()
        {
            Actual.MessageId.Should().StartWith("<");
            Actual.MessageId.Should().EndWith("@smtp-relay.sendinblue.com>");
        }
    }
}