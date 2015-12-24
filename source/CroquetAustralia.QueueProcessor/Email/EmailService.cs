using System.Collections.Generic;
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

        private static string Send(EmailMessage emailMessage) { 
            var mailInBlue = new MailInBlue(new MailInBlueSettings().AccessId);

            var to = emailMessage.To.ToDictionary(e => e.Email, e => e.Name);
            var fromName = new List<string>(new [] {emailMessage.From.Email, emailMessage.From.Name});
            var bcc = emailMessage.Bcc.ToDictionary(e => e.Email, e => e.Name);
            var bodyAsHtml = emailMessage.BodyAsHtml();
            var bodyAsText = emailMessage.BodyAsText();

            JObject response = mailInBlue.send_email(
                to, 
                emailMessage.Subject, 
                fromName, 
                bodyAsHtml, 
                bodyAsText, 
                null, 
                bcc, 
                null, 
                null, 
                null);

            var data = response["data"];
            var messageId = data["message-id"];

            return messageId.ToString();
        }
    }
}