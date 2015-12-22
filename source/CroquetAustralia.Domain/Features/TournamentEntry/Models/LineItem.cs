using System;
using System.ComponentModel.DataAnnotations;
using EmptyStringGuard;
using Newtonsoft.Json;
using NullGuard;
using ValidationFlags = NullGuard.ValidationFlags;

namespace CroquetAustralia.Domain.Features.TournamentEntry.Models
{
    [NullGuard(ValidationFlags.None)]
    [EmptyStringGuard(EmptyStringGuard.ValidationFlags.None)]
    public class LineItem
    {
        [Required]
        public Guid Id { get; set; }

        [Range(0, double.MaxValue)]
        public decimal UnitPrice { get; set; }

        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        [Range(0, 100)]
        public decimal DiscountPercentage { get; set; }

        [JsonIgnore]
        public decimal TotalPrice => Quantity * UnitPrice * (100 - DiscountPercentage) / 100;
    }
}