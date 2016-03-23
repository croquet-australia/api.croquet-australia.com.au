using System;
using NodaTime;

namespace CroquetAustralia.Domain.Data
{
    public class Tournament
    {
        public Tournament(string id, string title, ZonedDateTime starts, ZonedDateTime finishes, string location, TournamentItem[] events, ZonedDateTime eventsClose, TournamentItem[] functions, ZonedDateTime functionsClose, TournamentItem[] merchandise, ZonedDateTime merchandiseClose, bool isDoubles, string discipline, string slug, string depositStating)
        {
            Id = new Guid(id);
            Title = title;
            Starts = starts;
            Finishes = finishes;
            Location = location;
            Events = events;
            EventsClose = eventsClose;
            Functions = functions;
            FunctionsClose = functionsClose;
            Merchandise = merchandise;
            MerchandiseClose = merchandiseClose;
            IsDoubles = isDoubles;
            Discipline = discipline;
            Slug = slug;
            DepositStating = depositStating;
        }

        public Guid Id { get; }
        public string Title { get; }
        public ZonedDateTime Starts { get; }
        public ZonedDateTime Finishes { get; }
        public string Location { get; }
        public TournamentItem[] Events { get; }
        public ZonedDateTime EventsClose { get; }
        public TournamentItem[] Functions { get; }
        public ZonedDateTime FunctionsClose { get; }
        public TournamentItem[] Merchandise { get; }
        public ZonedDateTime MerchandiseClose { get; }
        public bool IsDoubles { get; }
        public string Discipline { get; }
        public string Slug { get; }
        public string DepositStating { get; }
    }
}