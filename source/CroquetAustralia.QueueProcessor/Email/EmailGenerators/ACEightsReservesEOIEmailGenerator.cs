using System.Linq;
using CroquetAustralia.Domain.Features.TournamentEntry.Events;

namespace CroquetAustralia.QueueProcessor.Email.EmailGenerators
{
    public class ACEightsReservesEOIEmailGenerator : BaseEmailGenerator
    {
        /* todo: remove hard coding of email addresses */
        private static readonly EmailAddress ACSC = new EmailAddress("acquinn@bigpond.com", "Croquet Australia - Chair AC Selection");

        private static readonly EmailAddress[] BCC =
        {
            new EmailAddress("admin@croquet-australia.com.au", "Croquet Australia"),
            ACSC
        };

        public ACEightsReservesEOIEmailGenerator(EmailMessageSettings emailMessageSettings)
            : base(emailMessageSettings, ACSC, GetBCC(emailMessageSettings))
        {
        }

        protected override string GetTemplateName(EntrySubmitted entrySubmitted)
        {
            return "ACEightsReservesEOI";
        }

        private static EmailAddress[] GetBCC(EmailMessageSettings emailMessageSettings)
        {
            return emailMessageSettings.Bcc.Any() ? BCC : new EmailAddress[] {};
        }
    }
}