using System;
using CroquetAustralia.Domain.Core;

namespace CroquetAustralia.Domain.Features.TournamentEntry
{
    public class Entry : IEntity
    {
        public Guid Id { get; set; }
    }
}
