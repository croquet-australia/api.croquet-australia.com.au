using System.Collections.Generic;

namespace CroquetAustralia.QueueProcessor.Email
{
    public class EmailMessage
    {
        public EmailMessage(IEnumerable<EmailAddress> to, IEnumerable<EmailAddress> cc, IEnumerable<EmailAddress> bcc, EmailAddress from, string subject, string bodyAsText, string bodyAsHtml, EmailAttachments attachments)
        {
            // todo: validate arguments
            To = to;
            Cc = cc;
            Bcc = bcc;
            From = from;
            Subject = subject;
            BodyAsText = bodyAsText;
            BodyAsHtml = bodyAsHtml;
            Attachments = attachments;
        }

        public IEnumerable<EmailAddress> To { get; }
        public IEnumerable<EmailAddress> Cc { get; }
        public IEnumerable<EmailAddress> Bcc { get; }
        public EmailAddress From { get; }
        public string Subject { get; }
        public string BodyAsText { get; }
        public string BodyAsHtml { get; }
        public EmailAttachments Attachments { get; }
    }
}