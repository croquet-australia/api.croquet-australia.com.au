using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using CroquetAustralia.Domain.Services.Repositories;
using CroquetAustralia.DownloadTournamentEntries.ReadModels;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace CroquetAustralia.DownloadTournamentEntries
{
    public class Program
    {
        public static void Main()
        {
            try
            {
                var tournamentIds = new[]
                {
                    Guid.Parse(TournamentsRepository.TournamentIdGcOpenDoubles2017),
                    Guid.Parse(TournamentsRepository.TournamentIdGcOpenSingles2017)
                };

                var tournamentEntries = GetTournamentEntries(tournamentIds);

                WriteTournamentEntries(tournamentEntries);
                WriteFunctions(tournamentEntries);
            }

            catch (Exception exception)
            {
                WriteException(exception);
            }
        }

        private static void WriteFunctions(IEnumerable<Model> tournamentEntries)
        {
            var functionLineItems = tournamentEntries.SelectMany(t => t.EntrySubmitted.Functions);
            var functions = functionLineItems.GroupBy(f => f.TournamentItem.Id);

            foreach (var function in functions)
            {
                WriteFunction(function);
            }
        }

        private static void WriteFunction(IEnumerable<FunctionReadModel> functions)
        {
            WriteFunction(functions.ToArray());
        }

        private static void WriteFunction(FunctionReadModel[] readModels)
        {
            Console.WriteLine();
            Console.WriteLine($"{readModels.First().TournamentItem.Title}");
            Console.WriteLine("First Name,Last Name,Quantity,Unit Price,Total Price, Dietary Requirements");

            foreach (var readModel in readModels)
            {
                Console.WriteLine($"\"{readModel.EntrySubmitted.Player.FirstName}\",\"{readModel.EntrySubmitted.Player.LastName}\",{readModel.LineItem.Quantity}, {readModel.LineItem.UnitPrice}, {readModel.LineItem.TotalPrice},\"{readModel.EntrySubmitted.DietaryRequirements}\"");
            }
        }

        private static void WriteTournamentEntries(IEnumerable<Model> tournamentEntries)
        {
            Console.WriteLine(Model.Headings);

            foreach (var tournamentEntry in tournamentEntries)
            {
                Console.WriteLine(tournamentEntry);
            }
        }

        private static Model[] GetTournamentEntries(Guid[] tournamentIds)
        {
            var tournamentsRepository = new TournamentsRepository();
            var connectionString = ConfigurationManager.AppSettings["ConnectionString:AzureStorage"];
            var storage = CloudStorageAccount.Parse(connectionString);
            var client = storage.CreateCloudTableClient();
            var table = client.GetTableReference("Events");
            var tableQuery = new TableQuery();
            var tableEntities = (
                from e in table.ExecuteQuery(tableQuery)
                group e by e.PartitionKey
                into g
                orderby g.Key
                select g).ToArray();


            var allModels = tableEntities
                .Select(group => new Model(@group.Key, @group.AsEnumerable(), tournamentsRepository))
                .ToArray();

            var orderedModels = allModels
                .OrderBy(m => m.EntrySubmitted.Tournament.Title)
                .ThenBy(m => m.EntrySubmitted.Created)
                .ToArray();

            var whereModels = orderedModels
                .Where(m => tournamentIds.Contains(m.EntrySubmitted.Tournament.Id) && !IsTestRecord(m))
                .ToArray();

            return whereModels;
        }

        private static void WriteException(Exception exception)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine();
            Console.WriteLine(exception.Message);

            if (exception.Data.Count > 0)
            {
                Console.WriteLine();
                foreach (var key in exception.Data.Keys)
                {
                    Console.WriteLine($"{key}: {exception.Data[key]}");
                }
            }

            Console.WriteLine("----------------------------------------------------------------");
            Console.WriteLine(exception);
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        private static bool IsTestRecord(Model model)
        {
            if (model.EntrySubmitted.Tournament.Id == Guid.Parse(TournamentsRepository.TournamentIdAcPatronsTrophy2016))
            {
                return model.EntrySubmitted.Player.Email == "pfreer@netspeed.com.au";
            }

            if (IsGcGenderOpen(model))
            {
                var testRecord = new DateTime(2016, 07, 13, 07, 13, 36);
                return model.EntrySubmitted.Created < testRecord;
            }

            if (model.EntrySubmitted.Player.Email.Contains("@example."))
            {
                return true;
            }

            return false;
        }

        private static bool IsGcGenderOpen(Model model)
        {
            return model.EntrySubmitted.Tournament.Id == Guid.Parse(TournamentsRepository.TournamentIdGcWomensOpen2016) || model.EntrySubmitted.Tournament.Id == Guid.Parse(TournamentsRepository.TournamentIdGcMensOpen2016);
        }
    }
}