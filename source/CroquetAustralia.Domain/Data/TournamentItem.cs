using System;

namespace CroquetAustralia.Domain.Data
{
    public class TournamentItem
    {
        public TournamentItem(string itemType, string id, string title, decimal unitPrice)
        {
            ItemType = itemType;
            Id = new Guid(id);
            Title = title;
            UnitPrice = unitPrice;
        }

        public string ItemType { get; }
        public Guid Id { get; }
        public string Title { get; }
        public decimal UnitPrice { get; }
    }
}