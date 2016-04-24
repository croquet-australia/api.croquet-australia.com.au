﻿using System;
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
                var tournamentIds = new[] { Guid.Parse(TournamentsRepository.TournamentIdGcOpenDoubles2016), Guid.Parse(TournamentsRepository.TournamentIdGcOpenSingles2016) };
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
                    .Where(m => tournamentIds.Contains(m.EntrySubmitted.Tournament.Id))
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
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine();
                Console.WriteLine(exception.Message);
                Console.WriteLine("----------------------------------------------------------------");
                Console.WriteLine(exception);
                Console.ForegroundColor = ConsoleColor.Gray;
            }
        }
    }
}