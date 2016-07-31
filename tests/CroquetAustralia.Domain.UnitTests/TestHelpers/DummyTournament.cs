using System;
using CroquetAustralia.Domain.Data;
using NodaTime;
using OpenMagic;
using OpenMagic.Extensions;

namespace CroquetAustralia.Domain.UnitTests.TestHelpers
{
    public class DummyTournament
    {
        public DummyTournament(DummyFactory dummy)
        {
            Id = Guid.NewGuid().ToString();
            Title = dummy.Value<string>();
            Date = dummy.Value<ZonedDateTime>();
            Location = dummy.Value<string>();
            TournamentItems = new TournamentItem[] {};
            IsDoubles = dummy.Value<bool>();
            Discipline = new[] {"gc", "ac"}.RandomItem();
            Slug = RandomString.Next(CharacterSets.LowerAtoZ);
            DepositStating = dummy.Value<string>();
        }

        public string DepositStating { get; set; }
        public string Slug { get; set; }
        public string Discipline { get; set; }
        public bool IsDoubles { get; set; }
        public TournamentItem[] TournamentItems { get; set; }
        public string Location { get; set; }
        public ZonedDateTime Date { get; set; }
        public string Title { get; set; }
        public string Id { get; set; }
        public ZonedDateTime Starts { get; set; }
        public ZonedDateTime Finishes { get; set; }
        public bool IsEOI { get; set; }
        public string[] RelatedTournamentIds { get; set; }
        public bool IsUnder21 { get; set; }

        public Tournament Build()
        {
            return new Tournament(Id, Title, Starts, Finishes, Location, TournamentItems, Date, TournamentItems, Date, TournamentItems, Date, IsDoubles, Discipline, Slug, DepositStating, RelatedTournamentIds, IsEOI, IsUnder21);
        }
    }
}