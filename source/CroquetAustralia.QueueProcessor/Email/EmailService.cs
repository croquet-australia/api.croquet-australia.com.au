using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace CroquetAustralia.QueueProcessor.Email
{
    public class EmailService : IEmailService
    {
        public Task<string> SendAsync(EmailMessage emailMessage)
        {
            // todo: proper async method
            return Task.FromResult(Send(emailMessage));
        }

        private static string Send(EmailMessage emailMessage)
        {
            var mailInBlue = new MailInBlue(new MailInBlueSettings().AccessId);

            var to = emailMessage.To.ToDictionary(e => e.Email, e => e.Name);
            var fromName = new List<string>(new[] {emailMessage.From.Email, emailMessage.From.Name});
            var bcc = emailMessage.Bcc.ToDictionary(e => e.Email, e => e.Name);
            var attachments = emailMessage.Attachments.ToDictionary(a => a.Name, a => ToBase64String(a.FullName));

            JObject response = mailInBlue.send_email(
                to,
                emailMessage.Subject,
                fromName,
                emailMessage.BodyAsHtml,
                emailMessage.BodyAsText,
                /*cc*/ null,
                bcc,
                /*replyTo*/ null,
                attachments,
                /*headers*/ null);

            var code = response["code"];

            if (code.Value<string>() != "success")
            {
                throw new SendEmailException(code, response["message"], response["data"], emailMessage);
            }

            var data = response["data"];
            var messageId = data["message-id"];

            return messageId.ToString();
        }

        private static string ToBase64String(string path)
        {
            var bytes = File.ReadAllBytes(path);
            var file = Convert.ToBase64String(bytes);

            return file;
        }
    }
}