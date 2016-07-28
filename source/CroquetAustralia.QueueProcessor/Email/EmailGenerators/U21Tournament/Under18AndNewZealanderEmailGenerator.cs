using System.Collections.Generic;
using CroquetAustralia.Domain.Features.TournamentEntry.Events;

namespace CroquetAustralia.QueueProcessor.Email.EmailGenerators.U21Tournament
{
    public class Under18AndNewZealanderEmailGenerator : BaseEmailGenerator
    {
        public Under18AndNewZealanderEmailGenerator(EmailMessageSettings emailMessageSettings)
            : base(emailMessageSettings)
        {
        }

        protected override IEnumerable<string> GetAttachmentNames()
        {
            yield return "U21Tournament.Under 18 Consent Form NZ.pdf";
        }

        protected override string GetTemplateName(EntrySubmitted entrySubmitted)
        {
            return "U21Tournament.Under 18 and New Zealander";
        }
    }
}