using System;
using CroquetAustralia.Domain.Core;
using CroquetAustralia.Domain.Features.TournamentEntry.Models;
using EmptyStringGuard;
using NullGuard;
using ValidationFlags = NullGuard.ValidationFlags;

namespace CroquetAustralia.Domain.Features.TournamentEntry.Events
{
    [NullGuard(ValidationFlags.None)]
    [EmptyStringGuard(EmptyStringGuard.ValidationFlags.None)]
    public class EntrySubmitted : EntryDto, IEvent
    {
        public EntrySubmitted()
        {
            Created = DateTime.UtcNow;
        }

        public DateTime Created { get; set; }
    }
}