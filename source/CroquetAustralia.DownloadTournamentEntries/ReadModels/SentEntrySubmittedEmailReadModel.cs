using System;
using System.Diagnostics.CodeAnalysis;
using CroquetAustralia.Domain.Features.TournamentEntry.Events;

namespace CroquetAustralia.DownloadTournamentEntries.ReadModels
{
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Local")]
    public class SentEntrySubmittedEmailReadModel
    {
        public SentEntrySubmittedEmailReadModel(SentEntrySubmittedEmail @event)
        {
            Created = @event.Created;
            EmailId = @event.EmailId;
        }

        public DateTime Created { get; private set; }
        public string EmailId { get; private set; }
    }
}