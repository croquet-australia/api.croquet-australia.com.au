using System;
using CroquetAustralia.Domain.Core;
using EmptyStringGuard;
using NullGuard;
using ValidationFlags = NullGuard.ValidationFlags;

namespace CroquetAustralia.Domain.Features.TournamentEntry.Commands
{
    [NullGuard(ValidationFlags.None)]
    [EmptyStringGuard(EmptyStringGuard.ValidationFlags.None)]
    public class ReceivePayment : ICommand
    {
        public PaymentMethod PaymentMethod { get; set; }
        public Guid EntityId { get; set; }
    }
}