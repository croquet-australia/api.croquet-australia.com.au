﻿using System.Collections.Generic;
using System.IO;
using CroquetAustralia.QueueProcessor.Email;

namespace CroquetAustralia.QueueProcessor.UnitTests.TestHelpers.Mocks
{
    internal class InMemorySentEmail
    {
        public InMemorySentEmail(EmailAddress from, IEnumerable<EmailAddress> to, IEnumerable<EmailAddress> bcc, string subject, string bodyHtml, string bodyText, IEnumerable<FileInfo> attachments)
        {
            From = from;
            To = to;
            Bcc = bcc;
            Subject = subject;
            BodyHtml = bodyHtml;
            BodyText = bodyText;
            Attachments = new EmailAttachments(attachments);
        }

        public EmailAddress From { get; }
        public IEnumerable<EmailAddress> To { get; }
        public IEnumerable<EmailAddress> Bcc { get; }
        public string Subject { get; }
        public string BodyHtml { get; }
        public string BodyText { get; }
        public EmailAttachments Attachments { get; }
    }
}