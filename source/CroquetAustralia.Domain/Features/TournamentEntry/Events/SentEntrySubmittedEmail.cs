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

        public SentEntrySubmittedEmail(string emailId, object emailMessage, EntrySubmitted @event)
        {
            EntityId = @event.EntityId;
            EmailId = emailId;
            EmailMessage = emailMessage;
            EntrySubmitted = @event;
        }

        public string EmailId { get; set; }
        public object EmailMessage { get; set; }
        public EntrySubmitted EntrySubmitted { get; set; }

        public Guid EntityId { get; set; }
        public DateTime Created { get; set; } = DateTime.UtcNow;
    }
}