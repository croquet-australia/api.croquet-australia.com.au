using System;
using System.Linq;

namespace CroquetAustralia.Domain.Data
{
    public static class Tournaments
    {
        public static readonly Tournament MensOpen;
        private static readonly Tournament WomensOpen;

        static Tournaments()
        {
            MensOpen = GetMensOpen();
            WomensOpen = GetWomensOpen();
        }

        private static Tournament GetMensOpen()
        {
            const string tournamentId = "813f8b2c-7d53-4af1-989b-164685584c83";
            const string tournamentTitle = "Australian AC Men's Singles";

            var events = new[]
            {
                new TournamentItem("event", "da76a935-1a5a-492d-9876-7dbc77149f48", "Main and Consolation Events", 85),
                new TournamentItem("event", "ab655591-4947-42ae-839a-ed1ecd633f23", "Main Event Only", 85),
                new TournamentItem("event", "c379a10a-19c3-41a7-b73f-cff17541dd73", "Plate Only", 42.50m)
            };

            return GetOpenTournament(tournamentId, tournamentTitle, events);
        }

        private static Tournament GetWomensOpen()
        {
            const string tournamentId = "9cc639a0-764f-4247-ae14-338fac804ba3";
            const string tournamentTitle = "Australian AC Women's Singles";

            var events = new[]
            {
                new TournamentItem("event", "a9d8475a-cd63-460d-aed9-b5eb82cd06c6", "Main and Consolation events", 85),
                new TournamentItem("event", "eb5be945-b65a-4f35-8b31-ea298dce72ea", "Main Event Only", 85),
                new TournamentItem("event", "697c794c-7fb7-4e7f-9397-2f7796525587", "Plate Only", 42.50m)
            };

            return GetOpenTournament(tournamentId, tournamentTitle, events);
        }

        private static Tournament GetOpenTournament(string tournamentId, string tournamentTitle, TournamentItem[] events)
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
                new DateTime(2016, 3, 12),
                new DateTime(2016, 3, 15),
                "Victorian Croquet Centre, Cairnlea, VIC",
                events,
                functions,
                merchandise
                );

            return tournament;
        }

        public static Tournament GetById(Guid tournamentId)
        {
            return GetAll().Single(t => t.Id == tournamentId);
        }

        public static Tournament[] GetAll() => new[] {MensOpen, WomensOpen};
    }
}