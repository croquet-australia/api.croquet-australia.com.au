using System;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using CroquetAustralia.Domain.Core;
using CroquetAustralia.Domain.Features.TournamentEntry.Events;
using EmptyStringGuard;
using Newtonsoft.Json;
using NullGuard;
using ValidationFlags = NullGuard.ValidationFlags;

namespace CroquetAustralia.Domain.Features.TournamentEntry.Commands
{
    [NullGuard(ValidationFlags.None)]
    [EmptyStringGuard(EmptyStringGuard.ValidationFlags.None)]
    public class SubmitEntry : CommandBase<SubmitEntryValidator>
    {
        static SubmitEntry()
        {
            Mapper.Initialize(cfg => cfg.CreateMap<SubmitEntry, EntrySubmitted>()
                .ForMember(entrySubmitted => entrySubmitted.Created, options => options.Ignore()));
        }

        public Guid TournamentId { get; set; }
        public Guid? EventId { get; set; }
        public string DietaryRequirements { get; set; }
        public bool PayingForPartner { get; set; }
        public LineItem[] Functions { get; set; }
        public LineItem[] Merchandise { get; set; }
        public PlayerClass Player { get; set; }
        public PlayerClass Partner { get; set; }
        public PaymentMethod PaymentMethod { get; set; }

        public EntrySubmitted ToEntrySubmitted()
        {
            return Mapper.Map<EntrySubmitted>(this);
        }

        [NullGuard(ValidationFlags.None)]
        [EmptyStringGuard(EmptyStringGuard.ValidationFlags.None)]
        public class LineItem
        {
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

        [NullGuard(ValidationFlags.None)]
        [EmptyStringGuard(EmptyStringGuard.ValidationFlags.None)]
        public class PlayerClass
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }
            public string Phone { get; set; }
            public decimal? Handicap { get; set; }
            public bool Under21 { get; set; }
            public bool FullTimeStudentUnder25 { get; set; }
        }
    }
}