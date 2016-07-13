using System;

namespace CroquetAustralia.Domain.Data
{
    public class TournamentItem
    {
        public TournamentItem(string itemType, string id, string title, decimal unitPrice, bool isInformationOnly = false)
        {
            ItemType = itemType;
            Id = new Guid(id);
            Title = title;
            UnitPrice = unitPrice;
            IsInformationOnly = isInformationOnly;
        }

        public Guid Id { get; }
        public string ItemType { get; }
        public string Title { get; }
        public decimal UnitPrice { get; }
        public bool IsInformationOnly { get; set; }
    }
}