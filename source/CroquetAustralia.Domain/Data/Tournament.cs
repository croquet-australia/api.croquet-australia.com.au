using System;

namespace CroquetAustralia.Domain.Data
{
    public class Tournament
    {
        public Tournament(string id, string title, DateTime startsOn, DateTime endsOn, string location, TournamentItem[] events, TournamentItem[] functions, TournamentItem[] merchandise)
        {
            Id = new Guid(id);
            Title = title;
            StartsOn = startsOn;
            EndsOn = endsOn;
            Location = location;
            Events = events;
            Functions = functions;
            Merchandise = merchandise;
        }

        public Guid Id { get; }
        public string Title { get; }
        public DateTime StartsOn { get; }
        public DateTime EndsOn { get; }
        public string Location { get; }
        public TournamentItem[] Events { get; }
        public TournamentItem[] Functions { get; }
        public TournamentItem[] Merchandise { get; }
    }
}