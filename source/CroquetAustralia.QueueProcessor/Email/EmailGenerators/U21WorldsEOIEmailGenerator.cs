using System.Linq;
using CroquetAustralia.Domain.Features.TournamentEntry.Events;

namespace CroquetAustralia.QueueProcessor.Email.EmailGenerators
{
    public class U21WorldsEOIEmailGenerator : BaseEmailGenerator
    {
        /* todo: remove hard coding of email addresses */
        private static readonly EmailAddress U21Coordinator = new EmailAddress("ndu21c@croquet-australia.com.au", "Croquet Australia - National Co-ordinator Under 21 Croquet");

        private static readonly EmailAddress[] BCC =
        {
            U21Coordinator,
            new EmailAddress("admin@croquet-australia.com.au", "Croquet Australia")
        };

        public U21WorldsEOIEmailGenerator(EmailMessageSettings emailMessageSettings)
            : base(emailMessageSettings, U21Coordinator, GetBCC(emailMessageSettings))
        {
        }

        protected override string GetTemplateName(EntrySubmitted entrySubmitted)
        {
            return "EOI";
        }

        private static EmailAddress[] GetBCC(EmailMessageSettings emailMessageSettings)
        {
            return emailMessageSettings.Bcc.Any() ? BCC : new EmailAddress[] {};
        }
    }
}