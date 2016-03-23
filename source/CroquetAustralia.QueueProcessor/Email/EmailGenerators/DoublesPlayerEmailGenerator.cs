using CroquetAustralia.Domain.Features.TournamentEntry.Events;

namespace CroquetAustralia.QueueProcessor.Email.EmailGenerators
{
    public class DoublesPlayerEmailGenerator : BaseEmailGenerator
    {
        public DoublesPlayerEmailGenerator(EmailMessageSettings emailMessageSettings)
            : base(emailMessageSettings)
        {
        }

        protected override string GetTemplateName(EntrySubmitted entrySubmitted)
        {
            return entrySubmitted.PayingForPartner
                ? "Doubles Event Only - Paying for yourself and your partner - Player"
                : "Doubles Event Only - Paying for yourself only - Player";
        }
    }
}