using System;
using System.Diagnostics.CodeAnalysis;
using CroquetAustralia.Domain.Features.TournamentEntry.Events;

namespace CroquetAustralia.DownloadTournamentEntries.ReadModels
{
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Local")]
    public class PaymentReceivedReadModel
    {
        public PaymentReceivedReadModel(PaymentReceived @event)
        {
            Created = @event.Created;
        }

        public DateTime Created { get; private set; }
    }
}