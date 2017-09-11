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
            TournamentItem = FindFirstFunction(tournament, lineItem);
            EntrySubmitted = entrySubmitted;
            LineItem = lineItem;
        }

        private static TournamentItem FindFirstFunction(Tournament tournament, SubmitEntry.LineItem lineItem)
        {
            try
            {
                return tournament.Functions.First(f => f.Id == lineItem.Id);
            }
            catch (Exception e)
            {
                throw new Exception($"Cannot find function '{lineItem.Id}' in tournament '{tournament.Title}'.", e);
            }
        }

        public EntrySubmittedReadModel EntrySubmitted { get; }
        public SubmitEntry.LineItem LineItem { get; }
        public TournamentItem TournamentItem { get; }
    }
}