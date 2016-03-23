using System;
using System.ComponentModel.DataAnnotations;
using EmptyStringGuard;
using NullGuard;
using ValidationFlags = NullGuard.ValidationFlags;

namespace CroquetAustralia.Domain.Features.TournamentEntry.Models
{
    [NullGuard(ValidationFlags.None)]
    [EmptyStringGuard(EmptyStringGuard.ValidationFlags.None)]
    public abstract class EntryDto
    {
        internal EntryDto()
        {
        }

        [Required]
        public Guid EntityId { get; set; }

        [Required]
        public Guid TournamentId { get; set; }

        public Guid? EventId { get; set; }

        public string DietaryRequirements { get; set; }

        public bool IsDoubles { get; set; }

        public bool PayingForPartner { get; set; }

        [Required]
        public LineItem[] Functions { get; set; }

        [Required]
        public LineItem[] Merchandise { get; set; }

        [Required]
        public Player Player { get; set; }

        [Required]
        public Player Partner { get; set; }

        [Required]
        public PaymentMethod PaymentMethod { get; set; }
    }
}