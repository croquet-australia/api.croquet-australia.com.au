using System;
using CroquetAustralia.Domain.Core;
using CroquetAustralia.Domain.Features.TournamentEntry.Models;

namespace CroquetAustralia.Domain.Features.TournamentEntry.Events
{
    public class EntrySubmitted : EntryDto, IEvent
    {
        public EntrySubmitted()
        {
            Created = DateTime.UtcNow;
        }

        public DateTime Created { get; set; }
    }
}