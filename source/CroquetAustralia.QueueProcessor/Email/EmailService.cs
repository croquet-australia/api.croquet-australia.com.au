using System;
using System.Threading.Tasks;

namespace CroquetAustralia.QueueProcessor.Email
{
    public class EmailService : IEmailService
    {
        public Task<string> SendAsync(EmailMessage emailMessage)
        {
            // todo
            return Task.FromResult(Guid.NewGuid().ToString());
        }
    }
}