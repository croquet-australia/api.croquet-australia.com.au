using System.Linq;
using CroquetAustralia.Domain.Features.TournamentEntry.Events;

namespace CroquetAustralia.QueueProcessor.Email.EmailGenerators
{
    public class ACWorlds2018EOIEmailGenerator : BaseEmailGenerator
    {
        private static readonly EmailAddress[] BCC =
        {
            new EmailAddress("admin@croquet-australia.com.au", "Croquet Australia")
        };

        public ACWorlds2018EOIEmailGenerator(EmailMessageSettings emailMessageSettings)
            : base(emailMessageSettings, BCC[0], GetBCC(emailMessageSettings))
        {
        }

        protected override string GetTemplateName(EntrySubmitted entrySubmitted)
        {
            return "ACWorlds2018EOI";
        }

        private static EmailAddress[] GetBCC(EmailMessageSettings emailMessageSettings)
        {
            return emailMessageSettings.Bcc.Any() ? BCC : new EmailAddress[] {};
        }
    }
}