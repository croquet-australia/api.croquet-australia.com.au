using CroquetAustralia.Domain.Services;

namespace CroquetAustralia.DownloadTournamentEntries
{
    public class AzureStorageConnectionString : IAzureStorageConnectionString
    {
        public AzureStorageConnectionString(string connectionString)
        {
            Value = connectionString;
        }

        public string Value { get; }
    }
}