using System;

namespace CroquetAustralia.Domain.Data
{
    public class TournamentItem
    {
        public TournamentItem(string itemType, string id, string title, decimal unitPrice, string currency = "AUD", bool isInformationOnly = false)
        {
            ItemType = itemType;
            Id = new Guid(id);
            Title = title;
            UnitPrice = unitPrice;
            Currency = currency;
            IsInformationOnly = isInformationOnly;
        }

        public Guid Id { get; }
        public string ItemType { get; }
        public string Title { get; }
        public decimal UnitPrice { get; }
        public string Currency { get; }
        public bool IsInformationOnly { get; }
    }
}