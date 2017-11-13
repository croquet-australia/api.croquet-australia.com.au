using System.Linq;
using CroquetAustralia.Domain.Features.TournamentEntry.Events;

namespace CroquetAustralia.QueueProcessor.Email.EmailGenerators
{
    public class GCEightsEOIEmailGenerator : BaseEmailGenerator
    {
        /* todo: remove hard coding of email addresses */
        private static readonly EmailAddress GCSC = new EmailAddress("glenleslie@bigpond.com", "Croquet Australia - Chair GC Selection");

        private static readonly EmailAddress[] BCC =
        {
            new EmailAddress("admin@croquet-australia.com.au", "Croquet Australia"),
            GCSC
        };

        public GCEightsEOIEmailGenerator(EmailMessageSettings emailMessageSettings)
            : base(emailMessageSettings, GCSC, GetBCC(emailMessageSettings))
        {
        }

        protected override string GetTemplateName(EntrySubmitted entrySubmitted)
        {
            return "GCEightsEOI";
        }

        private static EmailAddress[] GetBCC(EmailMessageSettings emailMessageSettings)
        {
            return emailMessageSettings.Bcc.Any() ? BCC : new EmailAddress[] {};
        }
    }
}