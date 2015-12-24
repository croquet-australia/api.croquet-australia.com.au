using System;
using CroquetAustralia.Domain.Core;
using EmptyStringGuard;
using NullGuard;
using ValidationFlags = NullGuard.ValidationFlags;

namespace CroquetAustralia.Domain.Features.TournamentEntry.Events
{
    [NullGuard(ValidationFlags.None)]
    [EmptyStringGuard(EmptyStringGuard.ValidationFlags.None)]
    public class PaymentReceived : IEvent
    {
        public PaymentReceived()
        {
        }

        public PaymentReceived(Guid entityId, PaymentMethod paymentMethod)
        {
            EntityId = entityId;
            PaymentMethod = paymentMethod;
        }

        public PaymentMethod PaymentMethod { get; set; }
        public Guid EntityId { get; set; }
        public DateTime Created { get; set; } = DateTime.UtcNow;
    }
}