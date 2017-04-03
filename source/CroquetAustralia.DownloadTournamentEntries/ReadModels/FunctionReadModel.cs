using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using CroquetAustralia.Domain.Data;
using CroquetAustralia.Domain.Features.TournamentEntry.Commands;

namespace CroquetAustralia.DownloadTournamentEntries.ReadModels
{
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Local")]
    public class FunctionReadModel
    {
        public FunctionReadModel(EntrySubmittedReadModel entrySubmitted, Tournament tournament, SubmitEntry.LineItem lineItem)
        {
            TournamentItem = tournament.Functions.First(f => f.Id == lineItem.Id);
            EntrySubmitted = entrySubmitted;
            LineItem = lineItem;
        }

        public EntrySubmittedReadModel EntrySubmitted { get; }
        public SubmitEntry.LineItem LineItem { get; }
        public TournamentItem TournamentItem { get; }
    }
}