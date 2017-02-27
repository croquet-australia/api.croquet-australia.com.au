using System.Collections.Generic;
using System.Linq;
using CroquetAustralia.Domain.Features.TournamentEntry.Events;

namespace CroquetAustralia.QueueProcessor.Email.EmailGenerators
{
    public class Gateball2017EmailGenerator : BaseEmailGenerator
    {
        private static readonly EmailAddress InfoAtGateballEmailAddress = new EmailAddress("info@gateball.com.au", "Croquet Australia - Gateball Championships");

        private static readonly EmailAddress[] BCC =
        {
            InfoAtGateballEmailAddress,
            new EmailAddress("admin@croquet-australia.com.au", "Croquet Australia")
        };

        public Gateball2017EmailGenerator(EmailMessageSettings emailMessageSettings)
            : base(emailMessageSettings, InfoAtGateballEmailAddress, GetBCC(emailMessageSettings))
        {
        }

        protected override IEnumerable<string> GetAttachmentNames()
        {
            yield return "2017 Gateball Championships Team Details.pdf";
        }

        protected override string GetTemplateName(EntrySubmitted entrySubmitted)
        {
            return "2017 Gateball Championships";
        }

        private static EmailAddress[] GetBCC(EmailMessageSettings emailMessageSettings)
        {
            return emailMessageSettings.Bcc.Any() ? BCC : new EmailAddress[] { };
        }
    }
}