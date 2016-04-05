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
            EireCupTeamsReception = new FunctionReadModel(source.Functions.SingleOrDefault(f => f.Id == Guid.Parse("e759a9cf-c2e1-4961-b6c6-4e2eefcc1a63")));
            EireCupPresentationDinner = new FunctionReadModel(source.Functions.SingleOrDefault(f => f.Id == Guid.Parse("40b86428-7a89-48b1-ac29-9f468440bc84")));
        }

        public PlayerReadModel Player { get; private set; }
        public string PaymentMethod { get; private set; }
        public EventReadModel Event { get; private set; }
        public DateTime Created { get; private set; }
        public TournamentReadModel Tournament { get; private set; }
        public FunctionReadModel EireCupTeamsReception { get; private set; }
        public FunctionReadModel EireCupPresentationDinner { get; private set; }
    }
}