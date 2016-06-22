using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CroquetAustralia.Domain.Core;
using CroquetAustralia.Domain.Data;
using CroquetAustralia.Domain.Exceptions;
using CroquetAustralia.Domain.Features.TournamentEntry.Commands;
using NodaTime;

namespace CroquetAustralia.Domain.Services.Repositories
{
    public class TournamentsRepository : ITournamentsRepository
    {
        public const string TournamentIdGcOpenDoubles2016 = "648CA595-184D-4966-A978-D09B510EF371";
        public const string TournamentIdGcOpenSingles2016 = "729A6539-40AD-40B9-BB77-257DCFC47D75";
        public const string TournamentIdAcPatronsTrophy2016 = "675d9e9f-6163-43ff-bec5-ba34840a9be1";

        private static readonly Tournament[] Tournaments;

        static TournamentsRepository()
        {
            Tournaments = new[]
            {
                GetAcMensOpen(),
                GetAcWomensOpen(),
                GetGcOpenDoubles(),
                GetGcOpenSingles(),
                GetAcPatronsTrophy()
            };
        }

        public Task<bool> ExistsAsync(Guid tournamentId)
        {
            return Task.FromResult(Tournaments.Any(t => t.Id == tournamentId));
        }

        public Task<Tournament> GetByIdAsync(Guid tournamentId)
        {
            return GetTournamentAsync(t => t.Id == tournamentId);
        }

        public Task<Tournament> GetBySlugAsync(int year, string discipline, string slug)
        {
            return GetTournamentAsync(t => t.Starts.Year == year && t.Discipline == discipline && t.Slug == slug);
        }

        public Task<Tournament> GetByUrlAsync(string url)
        {
            var parts = url.Split('/');

            return GetBySlugAsync(Convert.ToInt32(parts[0]), parts[1], parts[2]);
        }

        public Task<Tournament> FindByIdAsync(Guid tournamentId)
        {
            return Task.FromResult(Tournaments.SingleOrDefault(t => t.Id == tournamentId));
        }

        public Task<IEnumerable<Tournament>> GetAllAsync()
        {
            return Task.FromResult(Tournaments.AsEnumerable());
        }

        private static Task<Tournament> GetTournamentAsync(Func<Tournament, bool> where)
        {
            return Task.FromResult(GetTournament(where));
        }

        private static Tournament GetTournament(Func<Tournament, bool> where)
        {
            var tournament = Tournaments.SingleOrDefault(where);

            if (tournament == null)
            {
                throw new TournamentNotFoundException(where);
            }

            return tournament;
        }

        private static Tournament GetAcMensOpen()
        {
            const string tournamentId = "813f8b2c-7d53-4af1-989b-164685584c83";
            const string tournamentTitle = "Australian AC Men's Singles";

            var events = new[]
            {
                new TournamentItem("event", "da76a935-1a5a-492d-9876-7dbc77149f48", "Main and Consolation Events", 85),
                new TournamentItem("event", "ab655591-4947-42ae-839a-ed1ecd633f23", "Main Event Only", 85),
                new TournamentItem("event", "c379a10a-19c3-41a7-b73f-cff17541dd73", "Plate Only", 42.50m)
            };

            return GetAcGenderTournament(tournamentId, tournamentTitle, "mens-open", events);
        }

        private static Tournament GetAcWomensOpen()
        {
            const string tournamentId = "9cc639a0-764f-4247-ae14-338fac804ba3";
            const string tournamentTitle = "Australian AC Women's Singles";

            var events = new[]
            {
                new TournamentItem("event", "a9d8475a-cd63-460d-aed9-b5eb82cd06c6", "Main and Consolation events", 85),
                new TournamentItem("event", "eb5be945-b65a-4f35-8b31-ea298dce72ea", "Main Event Only", 85),
                new TournamentItem("event", "697c794c-7fb7-4e7f-9397-2f7796525587", "Plate Only", 42.50m)
            };

            return GetAcGenderTournament(tournamentId, tournamentTitle, "womens-open", events);
        }

        private static Tournament GetAcGenderTournament(string tournamentId, string tournamentTitle, string slug, TournamentItem[] events)
        {
            var functions = new[]
            {
                new TournamentItem("function", "e759a9cf-c2e1-4961-b6c6-4e2eefcc1a63", "Eire Cup Teams Reception - 6:30pm Tuesday 15 March by invitation", 0),
                new TournamentItem("function", "40b86428-7a89-48b1-ac29-9f468440bc84", "Eire Cup Presentation Dinner - 6:30pm Sunday 20 March", 50)
            };

            var merchandise = new TournamentItem[] {};

            var tournament = new Tournament(
                tournamentId,
                tournamentTitle,
                "12 Mar 2016 Australia/Melbourne".ToZonedDateTime(),
                "15 Mar 2016 Australia/Melbourne".ToZonedDateTime(),
                "Victorian Croquet Centre, Cairnlea, VIC",
                events,
                "18 Feb 2016 23:59 Australia/Perth".ToZonedDateTime(),
                functions,
                "15 Mar 2016 15:00 Australia/Perth".ToZonedDateTime(),
                merchandise,
                "15 Mar 2016 15:00 Australia/Perth".ToZonedDateTime(),
                false,
                "ac",
                slug,
                "AC Championship");

            return tournament;
        }

        private static Tournament GetAcPatronsTrophy()
        {
            const string tournamentId = TournamentIdAcPatronsTrophy2016;
            const string tournamentTitle = "Australian AC Patron's Trophy";
            const string location = "Moorabinda, Bunbury, Western Australia";
            const string slug = "patrons-trophy";
            const string depositStating = "Patron's Trophy";
            const string discipline = "ac";
            var starts = "11 Jun 2016 Australia/Perth".ToZonedDateTime();
            var finishes = "13 Jun 2016 Australia/Perth".ToZonedDateTime();
            var eventsClose = "02 Jun 2016 23:59:59 Australia/Perth".ToZonedDateTime();
            var functionsClose = eventsClose;
            var merchandiseClose = functionsClose;

            var events = new[]
            {
                new TournamentItem("event", "379d333b-afe1-4efd-bf89-4c30c4fd2c0c", "Main Event", 90)
            };

            var functions = new TournamentItem[] {};
            var merchandise = new TournamentItem[] {};

            var tournament = new Tournament(
                tournamentId,
                tournamentTitle,
                starts,
                finishes,
                location,
                events,
                eventsClose,
                functions,
                functionsClose,
                merchandise,
                merchandiseClose,
                false,
                discipline,
                slug,
                depositStating);

            return tournament;
        }

        private static Tournament GetGcOpenSingles()
        {
            const string tournamentTitle = "Australian GC Open Singles";
            const string slug = "open-singles";
            const bool isDoubles = false;
            var starts = "11 May 2016 Australia/Melbourne".ToZonedDateTime();
            var finishes = "15 May 2016 Australia/Melbourne".ToZonedDateTime();
            var eventsClose = "21 Apr 2016 17:00 Australia/Perth".ToZonedDateTime();
            var events = new[]
            {
                new TournamentItem("event", "2be6c66d-080f-4266-9523-b13948cbca90", "Main and Consolation Events", 132),
                new TournamentItem("event", "8dec18e3-34a0-4d3b-a8cb-35ca0798df8e", "Main Event Only", 132),
                new TournamentItem("event", "16836a4f-9b25-43c9-9e30-26f7f7e6879b", "Plate Only", 66)
            };

            return GetGcOpenTournament(TournamentIdGcOpenSingles2016, tournamentTitle, starts, finishes, events, eventsClose, isDoubles, slug);
        }

        private static Tournament GetGcOpenDoubles()
        {
            const string tournamentTitle = "Australian GC Open Doubles";
            const string slug = "open-doubles";
            const bool isDoubles = true;
            var starts = "07 May 2016 Australia/Melbourne".ToZonedDateTime();
            var finishes = "10 May 2016 Australia/Melbourne".ToZonedDateTime();
            var eventsClose = "21 Apr 2016 23:59 Australia/Perth".ToZonedDateTime();
            var events = new[]
            {
                new TournamentItem("event", "cb0f7155-0342-48b3-b1ef-eb719b171f28", "Main and Consolation Events", 80),
                new TournamentItem("event", "57517a05-af5b-49e8-80be-e5761f153638", "Main Event Only", 80),
                new TournamentItem("event", "1497b1db-86c0-47e3-adac-700110c6a0fa", "Plate Only", 40)
            };

            return GetGcOpenTournament(TournamentIdGcOpenDoubles2016, tournamentTitle, starts, finishes, events, eventsClose, isDoubles, slug);
        }

        private static Tournament GetGcOpenTournament(string tournamentId, string tournamentTitle, ZonedDateTime starts, ZonedDateTime finishes, TournamentItem[] events, ZonedDateTime eventsClose, bool isDoubles, string slug)
        {
            var functions = new TournamentItem[] {};
            var functionsClose = eventsClose;
            var merchandise = new TournamentItem[] {};
            var merchandiseClose = eventsClose;

            var tournament = new Tournament(
                tournamentId,
                tournamentTitle,
                starts,
                finishes,
                "Victorian Croquet Centre, Cairnlea, VIC",
                events,
                eventsClose,
                functions,
                functionsClose,
                merchandise,
                merchandiseClose,
                isDoubles,
                "gc",
                slug,
                "GC " + (isDoubles ? "Doubles" : "Singles"));

            return tournament;
        }
    }
}