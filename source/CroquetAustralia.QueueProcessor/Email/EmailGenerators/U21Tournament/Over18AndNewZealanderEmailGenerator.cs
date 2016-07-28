using CroquetAustralia.Domain.Features.TournamentEntry.Events;

namespace CroquetAustralia.QueueProcessor.Email.EmailGenerators.U21Tournament
{
    public class Over18AndNewZealanderEmailGenerator : BaseEmailGenerator
    {
        public Over18AndNewZealanderEmailGenerator(EmailMessageSettings emailMessageSettings)
            : base(emailMessageSettings)
        {
        }

        protected override string GetTemplateName(EntrySubmitted entrySubmitted)
        {
            return "U21Tournament.18-21 and New Zealander";
        }
    }
}