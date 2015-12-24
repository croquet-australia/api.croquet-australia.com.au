using System;
using CroquetAustralia.Domain.Core;
using EmptyStringGuard;
using NullGuard;
using ValidationFlags = NullGuard.ValidationFlags;

namespace CroquetAustralia.Domain.Features.TournamentEntry.Events
{
    [NullGuard(ValidationFlags.None)]
    [EmptyStringGuard(EmptyStringGuard.ValidationFlags.None)]
    public class SentEntrySubmittedEmail : IEvent
    {
        public SentEntrySubmittedEmail()
        {
        }

        public SentEntrySubmittedEmail(EntrySubmitted @event, string emailId)
        {
            EntityId = @event.EntityId;
            EmailId = emailId;
            EntrySubmitted = @event;
            Created = DateTime.UtcNow;
        }

        public Guid EntityId { get; set; }
        public string EmailId { get; set; }
        public EntrySubmitted EntrySubmitted { get; set; }
        public DateTime Created { get; set; }
    }
}