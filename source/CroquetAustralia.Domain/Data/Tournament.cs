using System;
using NodaTime;
using NullGuard;

namespace CroquetAustralia.Domain.Data
{
    public class Tournament
    {
        public Tournament(string id, string title, ZonedDateTime starts, ZonedDateTime finishes, string location, TournamentItem[] events, ZonedDateTime eventsClose, TournamentItem[] functions, ZonedDateTime functionsClose, TournamentItem[] merchandise, ZonedDateTime merchandiseClose, bool isDoubles, string discipline, string slug, [AllowNull] string depositStating, [AllowNull] string[] relatedTournamentIds = null, bool isEOI = false)
        {
            string validationMessage;

            // todo: Apply validation rule to deserialization
            if (!IsDepositStatingValid(depositStating, isEOI, out validationMessage))
            {
                throw new ArgumentNullException(nameof(depositStating), validationMessage);
            }

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
            RelatedTournamentIds = relatedTournamentIds ?? new string[] {};
            IsEOI = isEOI;
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
        public string DepositStating { [return: AllowNull] get; }
        public string[] RelatedTournamentIds { get; }
        public bool IsEOI { get; }

        private static bool IsDepositStatingValid(string depositStating, bool isEOI, out string validationMessage)
        {
            if (!string.IsNullOrWhiteSpace(depositStating) || isEOI)
            {
                validationMessage = null;
                return true;
            }

            validationMessage = "Value cannot be null when isEOI is false.";
            return false;
        }
    }
}