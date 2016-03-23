using CroquetAustralia.Domain.Features.TournamentEntry;
using CroquetAustralia.Domain.Features.TournamentEntry.Events;

namespace CroquetAustralia.QueueProcessor.Specifications.TestHelpers
{
    public class GivenData
    {
        public string TournamentSlug { get; set; }
        public bool PayingForPartner { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public EntrySubmitted EntrySubmitted { get; set; }
    }
}