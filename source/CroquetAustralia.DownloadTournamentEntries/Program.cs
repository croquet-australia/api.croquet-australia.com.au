using System;
using System.Configuration;
using System.Linq;
using CroquetAustralia.Domain.Services.Repositories;
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
                    Guid.Parse(TournamentsRepository.TournamentIdGcU21)
                };

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

                Console.WriteLine(Model.Headings);

                var models = tableEntities
                    .Select(group => new Model(group.Key, group.AsEnumerable(), tournamentsRepository))
                    .Where(m => tournamentIds.Contains(m.EntrySubmitted.Tournament.Id) && !IsTestRecord(m))
                    .ToArray();

                var orderedModels = models
                    .OrderBy(m => m.EntrySubmitted.Tournament.Title)
                    .ThenBy(m => m.EntrySubmitted.Created)
                    .ToArray();

                foreach (var model in orderedModels)
                {
                    Console.WriteLine(model);
                }
            }

            catch (Exception exception)
            {
                WriteException(exception);
            }
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

            return false;
        }

        private static bool IsGcGenderOpen(Model model)
        {
            return model.EntrySubmitted.Tournament.Id == Guid.Parse(TournamentsRepository.TournamentIdGcWomensOpen2016) || model.EntrySubmitted.Tournament.Id == Guid.Parse(TournamentsRepository.TournamentIdGcMensOpen2016);
        }
    }
}