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
        public const string TournamentIdGcOpenDoubles2016 = "648ca595-184d-4966-a978-d09b510ef371";
        public const string TournamentIdGcOpenSingles2016 = "729a6539-40ad-40b9-bb77-257dcfc47d75";
        public const string TournamentIdGcOpenDoubles2017 = "852f05a2-13e7-4b53-8e47-00cba0125511";
        public const string TournamentIdGcOpenSingles2017 = "63f43f13-fbdb-4f70-9b5e-b4a36e8ddc00";
        public const string TournamentIdAcPatronsTrophy2016 = "675d9e9f-6163-43ff-bec5-ba34840a9be1";
        public const string TournamentIdAcPatronsTrophy2017 = "563e4283-81a9-4cf8-a5a9-a1c9f6b67656";
        public const string TournamentIdAcPresidentsEightsEOI2016 = "35ac72fe-e3c3-402b-b48b-022412922cbc";
        public const string TournamentIdGcWorldsEOI2017 = "56111ebd-325f-4a68-95aa-35d3dfb7d5cc";
        public const string TournamentIdGcMensOpen2016 = "0b4a3868-c974-47bb-85d5-d6eaee6a67da";
        public const string TournamentIdGcWomensOpen2016 = "2ced7cd7-a505-497e-b628-1c31860d102b";
        public const string TournamentIdGcU21 = "d30d3ad9-b9ba-4a47-8545-f8e550aa9c6e";
        // ReSharper disable once InconsistentNaming
        public const string TournamentIdGcWorlds_U21_EOI_2017 = "3c04a403-2b2b-41b8-9163-3926c297e12d";
        public const string TournamentIdGcHandicapDoubles2016 = "591ce7dd-5e26-4a38-916f-0022a5533854";
        public const string TournamentIdGcHandicapSingles2016 = "e96ff546-1df2-478b-8faa-fa4d85869420";
        public const string TournamentIdGcEights2017EOI = "13b685d4-3734-4a59-b188-50333f104206";
        public const string TournamentIdAcOpenDoubles2016 = "587db6a8-5009-44a5-883f-17cec4a335df";
        public const string TournamentIdAcOpenSingles2016 = "31bf6160-b0d3-40e6-8ae9-7e689c659b33";
        public const string TournamentIdGcWorldQualifier2017EOI = "e777de8c-cd14-4e9f-afda-b0fae09ef549";
        public const string TournamentIdAcMensOpen2017 = "b06a8339-74c2-449f-a68e-70ba44c6f199";
        public const string TournamentIdAcWomensOpen2017 = "33b14886-2a59-4add-a798-e9a7d493af1e";
        public const string TournamentIdGateballChampionships2017 = "8161726d-ee57-4b0b-838b-6c0268de81bf";
        public const string TournamentIdAcWorldsEOI2018 = "8fbda7d9-efec-4386-b779-a04cd32755e5";
        public const string TournamentIdAcEights2017EOI = "77370e58-a06a-4816-be81-a3f7ad19ebb5";
        public const string TournamentIdGcMensOpen2017 = "9d7a4a85-8759-4bbb-9691-846803171a4f";
        public const string TournamentIdGcWomensOpen2017 = "f8c1fae1-1edc-4cac-8f5e-c3f678aa53a6";

        private static readonly Tournament[] Tournaments;

        static TournamentsRepository()
        {
            Tournaments = new[]
            {
                GetAcMensOpen2016(),
                GetAcWomensOpen2016(),
                GetGcOpenDoubles2016(),
                GetGcOpenSingles2016(),
                GetAcPatronsTrophy2016(),
                GetGcMensOpen2016(),
                GetGcWomensOpen2016(),
                GetGcAusU21(),
                GetGc_U21_WorldsEOI(),
                GetGcHandicapDoubles(),
                GetGcHandicapSingles(),
                GetGcEights2017EOI(),
                GetAcOpenDoubles(),
                GetAcOpenSingles(),
                GetGcWorldQualifier2017EOI(),
                GetAcMensOpen2017(),
                GetAcWomensOpen2017(),
                GetGateballChampionships2017(),
                GetGcOpenDoubles2017(),
                GetGcOpenSingles2017(),
                GetAcPatronsTrophy2017(),
                GetAcWorlds2018EOI(),
                GetAcEights2017EOI(),
                GetGcMensOpen2017(),
                GetGcWomensOpen2017()
            };
        }

        public Task<bool> ExistsAsync(Guid tournamentId)
        {
            return Task.FromResult(Tournaments.Any(t => t.Id == tournamentId));
        }

        public Task<Tournament> GetByIdAsync(Guid tournamentId)
        {
            try
            {
                return GetTournamentAsync(t => t.Id == tournamentId);
            }
            catch (TournamentNotFoundException exception)
            {
                throw new TournamentNotFoundException($"tournamentId == {tournamentId}", exception);
            }
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
            return Task.FromResult(FindByIdSync(tournamentId));
        }

        private static Tournament FindByIdSync(Guid tournamentId)
        {
            try
            {
                return Tournaments.SingleOrDefault(t => t.Id == tournamentId);
            }
            catch (Exception exception)
            {
                throw new Exception($"An error occurred while find tournamentId '{tournamentId}'.", exception);
            }
        }

        public Task<IEnumerable<Tournament>> GetAllAsync()
        {
            return Task.FromResult(Tournaments.AsEnumerable());
        }

        private static Tournament GetGcAusU21()
        {
            const string tournamentId = TournamentIdGcU21;
            const string tournamentTitle = "Australian Under 21 Golf Croquet Championship";

            var events = new[]
            {
                new TournamentItem("event", "353d908c-f9a3-4d5f-98c3-ae400408debd", "Main Event", 35)
            };

            var functions = new TournamentItem[] {};
            var merchandise = new TournamentItem[] {};

            var practiceStarts = "23 Sep 2016 Australia/Melbourne".ToZonedDateTime();
            var starts = "24 Sep 2016 Australia/Melbourne".ToZonedDateTime();
            var finishes = "26 Sep 2016 Australia/Melbourne".ToZonedDateTime();
            var eventsClose = "01 Sep 2016 23:59 Australia/Perth".ToZonedDateTime();
            var functionsClose = eventsClose;
            var merchandiseClose = functionsClose;
            const string location = "Victorian Croquet Centre, Cairnlea, Victoria";
            const string discipline = "gc";
            const bool isDoubles = false;
            const string depositStating = "AUS U21";
            const string slug = "u21";
            var relatedTournamentIds = new string[] {};
            const bool isUnder21 = true;

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
                isDoubles,
                discipline,
                slug,
                depositStating,
                relatedTournamentIds,
                isUnder21: isUnder21,
                practiceStarts: practiceStarts);

            return tournament;
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

        private static Tournament GetAcMensOpen2016()
        {
            const string tournamentId = "813f8b2c-7d53-4af1-989b-164685584c83";
            const string tournamentTitle = "Australian AC Men's Singles";

            var events = new[]
            {
                new TournamentItem("event", "da76a935-1a5a-492d-9876-7dbc77149f48", "Main and Consolation Events", 85),
                new TournamentItem("event", "ab655591-4947-42ae-839a-ed1ecd633f23", "Main Event Only", 85),
                new TournamentItem("event", "c379a10a-19c3-41a7-b73f-cff17541dd73", "Plate Only", 42.50m)
            };

            return GetAcGenderTournament2016(tournamentId, tournamentTitle, "mens-open", events);
        }

        private static Tournament GetAcWomensOpen2016()
        {
            const string tournamentId = "9cc639a0-764f-4247-ae14-338fac804ba3";
            const string tournamentTitle = "Australian AC Women's Singles";

            var events = new[]
            {
                new TournamentItem("event", "a9d8475a-cd63-460d-aed9-b5eb82cd06c6", "Main and Consolation events", 85),
                new TournamentItem("event", "eb5be945-b65a-4f35-8b31-ea298dce72ea", "Main Event Only", 85),
                new TournamentItem("event", "697c794c-7fb7-4e7f-9397-2f7796525587", "Plate Only", 42.50m)
            };

            return GetAcGenderTournament2016(tournamentId, tournamentTitle, "womens-open", events);
        }

        private static Tournament GetAcGenderTournament2016(string tournamentId, string tournamentTitle, string slug, TournamentItem[] events)
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

        private static Tournament GetAcPatronsTrophy2016()
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

        private static Tournament GetAcPatronsTrophy2017()
        {
            const string tournamentId = TournamentIdAcPatronsTrophy2017;
            const string tournamentTitle = "Australian AC Patron's Trophy";
            const string location = "Wynnum Croquet Club, Brisbane, Queensland";
            const string slug = "patrons-trophy";
            const string depositStating = "Patron's";
            const string discipline = "ac";
            var starts = "10 Jun 2017 Australia/Brisbane".ToZonedDateTime();
            var finishes = "12 Jun 2017 Australia/Brisbane".ToZonedDateTime();
            var eventsClose = "01 Jun 2017 23:59:59 Australia/Perth".ToZonedDateTime();
            var functionsClose = eventsClose;
            var merchandiseClose = functionsClose;

            var events = new[]
            {
                new TournamentItem("event", "3cca2917-03ec-4bf0-9b28-8541c9fac0ab", "Main Event", 90)
            };

            var functions = new TournamentItem[] { };
            var merchandise = new TournamentItem[] { };

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

        private static Tournament GetAcOpenSingles()
        {
            const string tournamentTitle = "Australian AC Open Singles";
            const string slug = "open-singles";
            const bool isDoubles = false;
            var starts = "22 Nov 2016 Australia/Melbourne".ToZonedDateTime();
            var finishes = "27 Nov 2016 Australia/Melbourne".ToZonedDateTime();
            var eventsClose = "03 Nov 2016 17:00 Australia/Perth".ToZonedDateTime();
            var events = new[]
            {
                new TournamentItem("event", "68b8de10-a68a-486d-b699-31f6d2e510ae", "Main and Consolation Events", 132),
                new TournamentItem("event", "e2a4fdfa-02df-4a69-9f54-ca5b185a05c9", "Main Event Only", 132),
                new TournamentItem("event", "d71af21f-a947-41f5-ac27-5b47b79fe728", "Plate Only", 66)
            };

            return GetAcOpenTournament(TournamentIdAcOpenSingles2016, tournamentTitle, starts, finishes, events, eventsClose, isDoubles, slug);
        }

        private static Tournament GetAcOpenDoubles()
        {
            const string tournamentTitle = "Australian AC Open Doubles";
            const string slug = "open-doubles";
            const bool isDoubles = true;
            var starts = "19 Nov 2016 Australia/Melbourne".ToZonedDateTime();
            var finishes = "21 Nov 2016 Australia/Melbourne".ToZonedDateTime();
            var eventsClose = "03 Nov 2016 23:59 Australia/Perth".ToZonedDateTime();
            var events = new[]
            {
                new TournamentItem("event", "d03dda72-61d1-4a21-9a31-28fce1d45471", "Main and Consolation Events", 80),
                new TournamentItem("event", "361b756d-4d72-4750-b6a6-409162d9bf00", "Main Event Only", 80),
                new TournamentItem("event", "e3b523a8-e171-44d4-ae6e-292f661ae6c2", "Plate Only", 40)
            };

            return GetAcOpenTournament(TournamentIdAcOpenDoubles2016, tournamentTitle, starts, finishes, events, eventsClose, isDoubles, slug);
        }

        private static Tournament GetGcOpenSingles2016()
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

            return GetGcOpenTournament2016(TournamentIdGcOpenSingles2016, tournamentTitle, starts, finishes, events, eventsClose, isDoubles, slug);
        }

        private static Tournament GetGcOpenDoubles2016()
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

            return GetGcOpenTournament2016(TournamentIdGcOpenDoubles2016, tournamentTitle, starts, finishes, events, eventsClose, isDoubles, slug);
        }

        private static Tournament GetAcOpenTournament(string tournamentId, string tournamentTitle, ZonedDateTime starts, ZonedDateTime finishes, TournamentItem[] events, ZonedDateTime eventsClose, bool isDoubles, string slug)
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
                "ac",
                slug,
                "AC " + (isDoubles ? "Doubles" : "Singles"));

            return tournament;
        }

        private static Tournament GetGcOpenTournament2016(string tournamentId, string tournamentTitle, ZonedDateTime starts, ZonedDateTime finishes, TournamentItem[] events, ZonedDateTime eventsClose, bool isDoubles, string slug)
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

        private static Tournament GetGcMensOpen2016()
        {
            const string tournamentId = TournamentIdGcMensOpen2016;
            const string tournamentTitle = "Australian GC Men's Singles";

            var events = new[]
            {
                new TournamentItem("event", "cc26f39f-c13c-43e7-9f0d-e171bdfcd53f", "Main and Consolation Events", 85),
                new TournamentItem("event", "9199c52d-faa2-40b9-82d8-68dfa2ac32fc", "Main Event Only", 85),
                new TournamentItem("event", "b89fa7d6-8a80-49d9-ad5a-fd40ed27966a", "Plate Only", 42.50m)
            };

            return GetGcGenderTournament2016(tournamentId, tournamentTitle, "mens-open", events, new[] {TournamentIdGcWomensOpen2016});
        }

        private static Tournament GetGcWomensOpen2016()
        {
            const string tournamentId = TournamentIdGcWomensOpen2016;
            const string tournamentTitle = "Australian GC Women's Singles";

            var events = new[]
            {
                new TournamentItem("event", "77a8e980-619a-40e0-b68e-a78b7d125e43", "Main and Consolation events", 85),
                new TournamentItem("event", "cb8390ad-6911-4885-af68-0d1cec84329e", "Main Event Only", 85),
                new TournamentItem("event", "4a492460-a1a8-4dfa-a952-21ff20b81ce8", "Plate Only", 42.50m)
            };

            return GetGcGenderTournament2016(tournamentId, tournamentTitle, "womens-open", events, new[] {TournamentIdGcMensOpen2016});
        }

        private static Tournament GetGcGenderTournament2016(string tournamentId, string tournamentTitle, string slug, TournamentItem[] events, string[] relatedTournamentIds)
        {
            var functions = new[]
            {
                new TournamentItem("function", "4d80520d-e8f2-4424-bdaf-164c774b8acd", "Welcome Men's & Women's Singles - 4:00pm Friday 2 September", 0),
                new TournamentItem("function", "7dbda0ca-7c20-4554-986e-77bce9bcd8dd", "Presentations and ISS Teams Reception - 6:00pm Tuesday 6 September by invitation", 0, isInformationOnly: true),
                new TournamentItem("function", "e81cf11f-fc5a-4d26-ba27-9792078c8ef0", "ISS Presentation Dinner - 7:00pm Sunday 11 September", 65)
            };

            var merchandise = new TournamentItem[] {};

            var starts = "03 Sep 2016 Australia/Perth".ToZonedDateTime();
            var finishes = "06 Sep 2016 Australia/Perth".ToZonedDateTime();
            var eventsClose = "05 Aug 2016 23:59 Australia/Perth".ToZonedDateTime();
            var functionsClose = "19 Aug 2016 23:59 Australia/Perth".ToZonedDateTime();
            var merchandiseClose = functionsClose;
            const string location = "Perth, WA";
            const string discipline = "gc";
            const bool isDoubles = false;
            const string depositStating = "GC Championship";

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
                isDoubles,
                discipline,
                slug,
                depositStating,
                relatedTournamentIds);

            return tournament;
        }

        private static Tournament GetGc_U21_WorldsEOI()
        {
            const string tournamentId = TournamentIdGcWorlds_U21_EOI_2017;
            const string tournamentTitle = "WCF Under 21's Golf Croquet World Championship - Expressions of Interest";
            const string location = "Victorian Croquet Centre, Cairnlea, VIC";
            const string slug = "u21-worlds-eoi";
            const string depositStating = null;
            const string discipline = "gc";
            const bool isDoubles = false;
            const bool isEOI = true;
            var starts = "18 Feb 2017 Australia/Melbourne".ToZonedDateTime();
            var finishes = "22 Feb 2017 Australia/Melbourne".ToZonedDateTime();
            var practiceStarts = "17 Feb 2017 Australia/Melbourne".ToZonedDateTime();
            var eventsClose = "14 Sep 2016 23:59:59 Australia/Perth".ToZonedDateTime();
            var functionsClose = eventsClose;
            var merchandiseClose = functionsClose;

            var events = new TournamentItem[] {};
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
                isDoubles,
                discipline,
                slug,
                depositStating,
                isEOI: isEOI,
                isUnder21: true,
                practiceStarts: practiceStarts);

            return tournament;
        }

        private static Tournament GetGcHandicapDoubles()
        {
            const string tournamentTitle = "Australian Golf Croquet Handicap Doubles";
            const string slug = "handicap-doubles";
            const bool isDoubles = true;
            var starts = "24 Oct 2016 Australia/Melbourne".ToZonedDateTime();
            var finishes = "25 Oct 2016 Australia/Melbourne".ToZonedDateTime();
            var eventsClose = "30 Sep 2016 23:59 Australia/Perth".ToZonedDateTime();
            var events = new[]
            {
                new TournamentItem("event", "6a87d376-f0cd-44c0-80d8-032ff44d75cb", "Main and Consolation Events", 35),
                new TournamentItem("event", "4093d035-d26a-475d-b747-1626c725f492", "Main Event Only", 35),
                new TournamentItem("event", "5060bfa7-d254-4955-a740-964a5112a549", "Plate Only", 17.50m)
            };

            return GetGcHandicapTournament(TournamentIdGcHandicapDoubles2016, tournamentTitle, starts, finishes, events, eventsClose, isDoubles, slug);
        }

        private static Tournament GetGcHandicapSingles()
        {
            const string tournamentTitle = "Australian Golf Croquet Handicap Singles";
            const string slug = "handicap-singles";
            const bool isDoubles = false;
            var starts = "25 Oct 2016 Australia/Melbourne".ToZonedDateTime();
            var finishes = "28 Oct 2016 Australia/Melbourne".ToZonedDateTime();
            var eventsClose = "30 Sep 2016 17:00 Australia/Perth".ToZonedDateTime();
            var events = new[]
            {
                new TournamentItem("event", "5b5b2e50-01ef-41bc-bce6-ccdd169a09d0", "Main and Consolation Events", 50),
                new TournamentItem("event", "c2081c70-c589-4880-b2d7-156322e4b15c", "Main Event Only", 50),
                new TournamentItem("event", "1b5e0ea9-a7f8-497a-a993-f9fd4a71fcb4", "Plate Only", 25)
            };

            return GetGcHandicapTournament(TournamentIdGcHandicapSingles2016, tournamentTitle, starts, finishes, events, eventsClose, isDoubles, slug);
        }

        private static Tournament GetGcHandicapTournament(string tournamentId, string tournamentTitle, ZonedDateTime starts, ZonedDateTime finishes, TournamentItem[] events, ZonedDateTime eventsClose, bool isDoubles, string slug)
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
                "Deniliquin Croquet Club, Deniliquin, NSW",
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

        private static Tournament GetGcEights2017EOI()
        {
            const string tournamentId = TournamentIdGcEights2017EOI;
            const string tournamentTitle = "Australian GC President's Eights - Expressions of Interest";
            const string location = "Victorian Croquet Centre, Cairnlea, VIC";
            const string slug = "presidents-eights-expressions-of-interest";
            const string depositStating = null;
            const string discipline = "gc";
            const bool isDoubles = false;
            const bool isEOI = true;
            var starts = "03 Feb 2017 Australia/Melbourne".ToZonedDateTime();
            var finishes = "05 Feb 2017 Australia/Melbourne".ToZonedDateTime();
            var eventsClose = "09 Nov 2016 23:59:59 Australia/Perth".ToZonedDateTime();
            var functionsClose = eventsClose;
            var merchandiseClose = functionsClose;

            var events = new TournamentItem[] {};
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
                isDoubles,
                discipline,
                slug,
                depositStating,
                isEOI: isEOI);

            return tournament;
        }

        private static Tournament GetGcWorldQualifier2017EOI()
        {
            const string tournamentId = TournamentIdGcWorldQualifier2017EOI;
            const string tournamentTitle = "WCF GC World Championship 2017 - Qualifying Tournament - Expressions of Interest";
            const string location = "Victorian Croquet Centre, Cairnlea, VIC";
            const string slug = "wcf-world-championship-qualifying-tournament-eoi";
            const string depositStating = null;
            const string discipline = "gc";
            const bool isDoubles = false;
            const bool isEOI = true;
            var starts = "20 Feb 2017 Australia/Melbourne".ToZonedDateTime();
            var finishes = "23 Feb 2017 Australia/Melbourne".ToZonedDateTime();
            var eventsClose = "05 Feb 2017 23:59:59 Australia/Perth".ToZonedDateTime();
            var functionsClose = eventsClose;
            var merchandiseClose = functionsClose;

            var events = new TournamentItem[] {};
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
                isDoubles,
                discipline,
                slug,
                depositStating,
                isEOI: isEOI);

            return tournament;
        }

        private static Tournament GetAcMensOpen2017()
        {
            const string tournamentId = TournamentIdAcMensOpen2017;
            const string tournamentTitle = "Australian AC Men's Singles";

            var events = new[]
            {
                new TournamentItem("event", "749aa8fa-84d1-4169-b53a-1f2fa33378f9", "Main and Consolation Events", 85),
                new TournamentItem("event", "b28c87e7-c16d-43a1-a7ad-dd2afea787c0", "Main Event Only", 85),
                new TournamentItem("event", "fc760925-eb1b-4d2e-97e7-fe8c137df5ca", "Plate Only", 42.50m)
            };

            return GetAcGenderTournament2017(tournamentId, tournamentTitle, "mens-open", events, "Hobart, Tasmania", false);
        }

        private static Tournament GetAcWomensOpen2017()
        {
            const string tournamentId = TournamentIdAcWomensOpen2017;
            const string tournamentTitle = "Australian AC Women's Singles";

            var events = new[]
            {
                new TournamentItem("event", "665efdec-eb50-45b5-9368-d6ed29fcf132", "Main and Consolation events", 85),
                new TournamentItem("event", "eb0aeea7-cc7d-49e1-be03-8c88f17422b3", "Main Event Only", 85),
                new TournamentItem("event", "635b4efc-5ab7-475b-9f1f-61baa210c1a7", "Plate Only", 42.50m)
            };

            return GetAcGenderTournament2017(tournamentId, tournamentTitle, "womens-open", events, "St Leonards Croquet Club, Launceston, Tasmania", true);
        }

        private static Tournament GetAcGenderTournament2017(string tournamentId, string tournamentTitle, string slug, TournamentItem[] events, string location, bool womens)
        {
            var functions = womens ? AcWomensEvents2017() : AcMensEvents2017();
            var merchandise = new TournamentItem[] {};

            var eventsClose = "16 Feb 2017 23:59 Australia/Perth".ToZonedDateTime();
            var functionsClose = "10 Mar 2017 23:59 Australia/Perth".ToZonedDateTime();
            var merchandiseClose = functionsClose;

            var tournament = new Tournament(
                tournamentId,
                tournamentTitle,
                "18 Mar 2017 Australia/Hobart".ToZonedDateTime(),
                "21 Mar 2017 Australia/Hobart".ToZonedDateTime(),
                location,
                events,
                eventsClose,
                functions,
                functionsClose,
                merchandise,
                merchandiseClose,
                false,
                "ac",
                slug,
                "AC Championship");

            return tournament;
        }

        private static TournamentItem[] AcMensEvents2017()
        {
            return new[]
            {
                new TournamentItem("function", "d83d300b-cc81-4fc4-bc43-01573a23cc3d", "Welcome BBQ - 5:00pm 17 March at Sandy Bay CC", 15),
                EireCupDinner2017()
            };
        }

        private static TournamentItem[] AcWomensEvents2017()
        {
            return new[]
            {
                new TournamentItem("function", "65d03cf0-f0cd-46ab-8585-a228954794f6", "Welcome High Tea at 5:00pm Friday 17 March at St Leonards CC", 15),
                EireCupDinner2017()
            };
        }

        private static TournamentItem EireCupDinner2017()
        {
            return new TournamentItem("function", "5aaaecb6-c6b4-4fed-9e26-a82a56a318eb", "Eire Cup Presentation Dinner - 7:00pm Sunday 26 March", 60);
        }

        private static Tournament GetGateballChampionships2017()
        {
            const string tournamentTitle = "Australian Gateball Championships";
            const string slug = "championships";
            const bool isDoubles = false;
            const string depositStating = "Gateball Championships";
            const string discipline = "gb";

            var starts = "13 Oct 2017 Australia/Sydney".ToZonedDateTime();
            var finishes = "15 Oct 2017 Australia/Sydney".ToZonedDateTime();
            var eventsClose = "22 Aug 2017 23:59 Australia/Perth".ToZonedDateTime();
            var events = new[]
            {
                new TournamentItem("event", "71afc255-9875-4373-a3dd-2e9423013d30", "Team Entry", 175)
            };

            var functions = new TournamentItem[] {};
            var functionsClose = eventsClose;
            var merchandise = new TournamentItem[] {};
            var merchandiseClose = eventsClose;

            var tournament = new Tournament(
                TournamentIdGateballChampionships2017,
                tournamentTitle,
                starts,
                finishes,
                "Bateau Bay, New South Wales",
                events,
                eventsClose,
                functions,
                functionsClose,
                merchandise,
                merchandiseClose,
                isDoubles,
                discipline,
                slug,
                depositStating);

            return tournament;
        }

        private static Tournament GetGcOpenDoubles2017()
        {
            const string tournamentTitle = "Australian GC Open Doubles";
            const string slug = "open-doubles";
            const bool isDoubles = true;
            var starts = "06 May 2017 Australia/Melbourne".ToZonedDateTime();
            var finishes = "09 May 2017 Australia/Melbourne".ToZonedDateTime();
            var eventsClose = "20 Apr 2017 23:59 Australia/Perth".ToZonedDateTime();
            var events = new[]
            {
                new TournamentItem("event", "f585ec96-77fb-4362-a171-3c8d28cb1eee", "Main and Consolation Events", 80),
                new TournamentItem("event", "251bd2bb-bbbe-4c80-b4c9-a65b9951067e", "Main Event Only", 80),
                new TournamentItem("event", "ec26a9d8-f4af-4f13-82b9-90d06fb69df6", "Plate Only", 40)
            };

            return GetGcOpenTournament2017(TournamentIdGcOpenDoubles2017, tournamentTitle, starts, finishes, events, eventsClose, isDoubles, slug);
        }

        private static Tournament GetGcOpenSingles2017()
        {
            const string tournamentTitle = "Australian GC Open Singles";
            const string slug = "open-singles";
            const bool isDoubles = false;
            var starts = "10 May 2017 Australia/Melbourne".ToZonedDateTime();
            var finishes = "14 May 2017 Australia/Melbourne".ToZonedDateTime();
            var eventsClose = "20 Apr 2017 23:59 Australia/Perth".ToZonedDateTime();
            var events = new[]
            {
                new TournamentItem("event", "a8320536-20ef-4e7d-ad99-c415b9a101e5", "Main and Consolation Events", 132),
                new TournamentItem("event", "c128d9f6-52e1-49d2-8fd8-1a3cc0047498", "Main Event Only", 132),
                new TournamentItem("event", "534b9e84-9670-48f9-a26b-a2c7cf11c302", "Plate Only", 66)
            };

            return GetGcOpenTournament2017(TournamentIdGcOpenSingles2017, tournamentTitle, starts, finishes, events, eventsClose, isDoubles, slug);
        }

        private static Tournament GetGcOpenTournament2017(string tournamentId, string tournamentTitle, ZonedDateTime starts, ZonedDateTime finishes, TournamentItem[] events, ZonedDateTime eventsClose, bool isDoubles, string slug)
        {
            var functions = new TournamentItem[] { };
            var functionsClose = eventsClose;
            var merchandise = new TournamentItem[] { };
            var merchandiseClose = eventsClose;
            var depositStating = "GC" + (isDoubles ? "D" : "S");
            const string location = "Victorian Croquet Centre, Cairnlea, VIC";
            const string discipline = "gc";

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
                isDoubles,
                discipline,
                slug,
                depositStating);

            return tournament;
        }

        private static Tournament GetAcWorlds2018EOI()
        {
            const string tournamentId = TournamentIdAcWorldsEOI2018;
            const string tournamentTitle = "WCF AC World Championship 2018 - Expressions of Interest";
            const string location = "Kelburn Croquet Club, Wellington, NZ";
            const string slug = "wcf-world-championship-eoi";
            const string depositStating = null;
            const string discipline = "ac";
            const bool isDoubles = false;
            const bool isEOI = true;
            var starts = "03 Feb 2018 Pacific/Auckland".ToZonedDateTime();
            var finishes = "11 Feb 2018 Pacific/Auckland".ToZonedDateTime();
            var eventsClose = "09 Jul 2017 23:59:59 Australia/Perth".ToZonedDateTime();
            var functionsClose = eventsClose;
            var merchandiseClose = functionsClose;

            var events = new TournamentItem[] { };
            var functions = new TournamentItem[] { };
            var merchandise = new TournamentItem[] { };

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
                isDoubles,
                discipline,
                slug,
                depositStating,
                isEOI: isEOI);

            return tournament;
        }

        private static Tournament GetAcEights2017EOI()
        {
            const string tournamentId = TournamentIdAcEights2017EOI;
            const string tournamentTitle = "Australian AC President's Eights - Expressions of Interest";
            const string location = "Victorian Croquet Centre, Cairnlea, VIC";
            const string slug = "presidents-eights-expressions-of-interest";
            const string depositStating = null;
            const string discipline = "ac";
            const bool isDoubles = false;
            const bool isEOI = true;
            var starts = "06 Oct 2017 Australia/Melbourne".ToZonedDateTime();
            var finishes = "09 Oct 2017 Australia/Melbourne".ToZonedDateTime();
            var eventsClose = "24 Jul 2017 23:59:59 Australia/Perth".ToZonedDateTime();
            var functionsClose = eventsClose;
            var merchandiseClose = functionsClose;

            var events = new TournamentItem[] { };
            var functions = new TournamentItem[] { };
            var merchandise = new TournamentItem[] { };

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
                isDoubles,
                discipline,
                slug,
                depositStating,
                isEOI: isEOI);

            return tournament;
        }

        private static Tournament GetGcMensOpen2017()
        {
            const string tournamentId = TournamentIdGcMensOpen2017;
            const string tournamentTitle = "Australian GC Men's Singles";

            var events = new[]
            {
                new TournamentItem("event", "765baf6a-0b9b-44b7-8343-2f4c53e705d4", "Main and Consolation Events", 85),
                new TournamentItem("event", "f4f95439-b756-4bd0-a03e-8527ec67cf96", "Main Event Only", 85),
                new TournamentItem("event", "45afbc57-d1d2-4a60-826b-d590c4fd656a", "Plate Only", 42.50m)
            };

            return GetGcGenderTournament2017(tournamentId, tournamentTitle, "mens-open", events, new[] { TournamentIdGcWomensOpen2017 }, "GC Mens");
        }

        private static Tournament GetGcWomensOpen2017()
        {
            const string tournamentId = TournamentIdGcWomensOpen2017;
            const string tournamentTitle = "Australian GC Women's Singles";

            var events = new[]
            {
                new TournamentItem("event", "aaa80a0c-c37e-47f5-835a-d00a22950cdd", "Main and Consolation events", 85),
                new TournamentItem("event", "aea02bf2-a321-493b-8720-26b728015c1a", "Main Event Only", 85),
                new TournamentItem("event", "ee1cb889-f5cc-4400-8e7c-0e9bc03f5fe2", "Plate Only", 42.50m)
            };

            return GetGcGenderTournament2017(tournamentId, tournamentTitle, "womens-open", events, new[] { TournamentIdGcMensOpen2017 }, "GC Womens");
        }

        private static Tournament GetGcGenderTournament2017(string tournamentId, string tournamentTitle, string slug, TournamentItem[] events, string[] relatedTournamentIds, string depositStating)
        {
            var functions = new[]
            {
                new TournamentItem("function", "eeb28e49-1978-4cdd-935c-fbe4c7e427f4", "Welcome BBQ Men's & Women's Singles - 5:00pm Friday 1 September", 10),
                new TournamentItem("function", "57388a06-7071-4e73-b8e4-2d05781f1780", "ISS Presentation Dinner - 6:30pm Sunday 10 September at Mercure Brisbane, North Quay", 60)
            };

            var merchandise = new TournamentItem[] { };

            var starts = "02 Sep 2017 Australia/Brisbane".ToZonedDateTime();
            var finishes = "05 Sep 2017 Australia/Brisbane".ToZonedDateTime();
            var eventsClose = "03 Aug 2017 23:59 Australia/Perth".ToZonedDateTime();
            var functionsClose = "27 Aug 2017 23:59 Australia/Perth".ToZonedDateTime();
            var merchandiseClose = functionsClose;
            const string location = "Brisbane, QLD";
            const string discipline = "gc";
            const bool isDoubles = false;

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
                isDoubles,
                discipline,
                slug,
                depositStating,
                relatedTournamentIds);

            return tournament;
        }
    }
}
