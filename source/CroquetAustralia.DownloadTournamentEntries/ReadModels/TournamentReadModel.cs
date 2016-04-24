using System;
using System.Diagnostics.CodeAnalysis;
using CroquetAustralia.Domain.Data;

namespace CroquetAustralia.DownloadTournamentEntries.ReadModels
{
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Local")]
    public class TournamentReadModel
    {
        public TournamentReadModel(Tournament tournament)
        {
            Id = tournament.Id;
            Title = tournament.Title;
            IsDoubles = tournament.IsDoubles;
        }

        public Guid Id { get; }
        public string Title { get; }
        public bool IsDoubles { get; }
    }
}