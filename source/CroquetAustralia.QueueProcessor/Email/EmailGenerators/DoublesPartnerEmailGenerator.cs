using CroquetAustralia.Domain.Features.TournamentEntry.Events;

namespace CroquetAustralia.QueueProcessor.Email.EmailGenerators
{
    public class DoublesPartnerEmailGenerator : BaseEmailGenerator
    {
        public DoublesPartnerEmailGenerator(EmailMessageSettings emailMessageSettings)
            : base(emailMessageSettings)
        {
        }

        protected override string GetTemplateName(EntrySubmitted entrySubmitted)
        {
            return entrySubmitted.PayingForPartner
                ? "Doubles Event Only - Paying for yourself and your partner - Partner"
                : "Doubles Event Only - Paying for yourself only - Partner";
        }
    }
}