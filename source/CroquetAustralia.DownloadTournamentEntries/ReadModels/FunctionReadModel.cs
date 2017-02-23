using System;
using System.Diagnostics.CodeAnalysis;
using CroquetAustralia.Domain.Features.TournamentEntry.Commands;

namespace CroquetAustralia.DownloadTournamentEntries.ReadModels
{
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Local")]
    public class FunctionReadModel
    {
        public FunctionReadModel(SubmitEntry.LineItem lineItem)
        {
            Id = lineItem.Id;
            Quantity = lineItem.Quantity;
        }

        public Guid Id { get; private set; }
        public int Quantity { get; private set; }
    }
}