using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CroquetAustralia.QueueProcessor.Email
{
    public interface IEmailService
    {
        Task<string> SendAsync(EmailMessage emailMessage);
    }
}