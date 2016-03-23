using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CroquetAustralia.QueueProcessor.Email;

namespace CroquetAustralia.QueueProcessor.UnitTests.TestHelpers.Mocks
{
    internal class InMemoryEmailService : IEmailService
    {
        private readonly Dictionary<string, InMemorySentEmail> _sentEmails = new Dictionary<string, InMemorySentEmail>();

        public IReadOnlyDictionary<string, InMemorySentEmail> SentEmails => (IReadOnlyDictionary<string, InMemorySentEmail>)_sentEmails.AsEnumerable();

        public Task<string> SendAsync(EmailMessage emailMessage)
        {
            var id = Guid.NewGuid().ToString();
            var sentEmail = new InMemorySentEmail(
                emailMessage.From,
                emailMessage.To,
                emailMessage.Bcc,
                emailMessage.Subject,
                emailMessage.BodyAsHtml,
                emailMessage.BodyAsText);

            _sentEmails.Add(id, sentEmail);

            return Task.FromResult(id);
        }
    }
}