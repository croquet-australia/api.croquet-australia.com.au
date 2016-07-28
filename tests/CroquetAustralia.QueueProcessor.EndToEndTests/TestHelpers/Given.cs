using CroquetAustralia.QueueProcessor.Email;

namespace CroquetAustralia.QueueProcessor.EndToEndTests.TestHelpers
{
    public class Given
    {
        public Given()
        {
            EmailMessageSettings = new EmailMessageSettings();
            EmailService = new EmailService();
        }

        public EmailMessageSettings EmailMessageSettings { get; set; }
        public EmailMessage EmailMessage { get; set; }
        public EmailService EmailService { get; set; }
    }
}