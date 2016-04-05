using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using CroquetAustralia.Domain.Data;

namespace CroquetAustralia.DownloadTournamentEntries.ReadModels
{
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Local")]
    public class EventReadModel
    {
        public EventReadModel(Guid? eventId, Tournament tournament)
        {
            if (eventId.HasValue)
            {
                Title = tournament.Events.Single(e => e.Id == eventId.Value).Title;
            }
        }

        public string Title { get; private set; }
    }
}