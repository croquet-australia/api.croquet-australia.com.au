using System.Linq;
using CroquetAustralia.Domain.Features.TournamentEntry.Events;

namespace CroquetAustralia.QueueProcessor.Email.EmailGenerators
{
    public class GCWorldQualifier2017EOIEmailGenerator : BaseEmailGenerator
    {
        private static readonly EmailAddress[] BCC =
        {
            new EmailAddress("admin@croquet-australia.com.au", "Croquet Australia")
        };

        public GCWorldQualifier2017EOIEmailGenerator(EmailMessageSettings emailMessageSettings)
            : base(emailMessageSettings, BCC[0], GetBCC(emailMessageSettings))
        {
        }

        protected override string GetTemplateName(EntrySubmitted entrySubmitted)
        {
            return "GCWorldQualifier2017EOI";
        }

        private static EmailAddress[] GetBCC(EmailMessageSettings emailMessageSettings)
        {
            return emailMessageSettings.Bcc.Any() ? BCC : new EmailAddress[] {};
        }
    }
}