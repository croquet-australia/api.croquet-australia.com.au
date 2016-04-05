using System.Diagnostics.CodeAnalysis;
using CroquetAustralia.Domain.Data;

namespace CroquetAustralia.DownloadTournamentEntries.ReadModels
{
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Local")]
    public class TournamentReadModel
    {
        public TournamentReadModel(Tournament tournament)
        {
            Title = tournament.Title;
        }

        public string Title { get; private set; }
    }
}