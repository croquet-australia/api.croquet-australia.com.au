using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using CroquetAustralia.Domain.Data;
using CroquetAustralia.Domain.Features.TournamentEntry.Events;

namespace CroquetAustralia.DownloadTournamentEntries.ReadModels
{
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Local")]
    public class EntrySubmittedReadModel
    {
        public EntrySubmittedReadModel(EntrySubmitted source, Tournament tournament)
        {
            Player = new PlayerReadModel(source.Player);
            PaymentMethod = source.PaymentMethod.ToString();
            Event = new EventReadModel(source.EventId, tournament);
            Created = source.Created;
            Tournament = new TournamentReadModel(tournament);
            PayingForPartner = source.PayingForPartner;
            Partner = source.Partner == null ? null : new PlayerReadModel(source.Partner);
            Functions = source.Functions.Select(function => new FunctionReadModel(tournament, function)).ToArray();
            DietaryRequirements = source.DietaryRequirements;
        }

        public PlayerReadModel Player { get; private set; }
        public string PaymentMethod { get; private set; }
        public EventReadModel Event { get; private set; }
        public DateTime Created { get; private set; }
        public TournamentReadModel Tournament { get; private set; }
        public bool PayingForPartner { get; private set; }
        public PlayerReadModel Partner { get; private set; }
        public FunctionReadModel[] Functions { get; private set; }
        public string DietaryRequirements { get; private set; }
    }
}