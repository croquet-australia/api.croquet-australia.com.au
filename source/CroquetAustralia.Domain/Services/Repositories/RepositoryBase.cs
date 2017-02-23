using System;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace CroquetAustralia.Domain.Services.Repositories
{
    public abstract class RepositoryBase
    {
        private readonly Lazy<CloudTable> _lazyTable;

        protected RepositoryBase(string tableName, IAzureStorageConnectionString connectionString)
        {
            _lazyTable = new Lazy<CloudTable>(() => GetTable(tableName, connectionString.Value));
        }

        protected CloudTable CloudTable => _lazyTable.Value;

        protected async Task AddAsync(ITableEntity tableEntity)
        {
            var tableOperation = TableOperation.Insert(tableEntity);
            await CloudTable.ExecuteAsync(tableOperation);
        }

        private CloudTable GetTable(string tableName, string connectionString)
        {
            var storageAccount = CloudStorageAccount.Parse(connectionString);
            var tableClient = storageAccount.CreateCloudTableClient();

            var table = tableClient.GetTableReference(tableName);
            table.CreateIfNotExists();

            return table;
        }

        public static string CreateTimestampRowKey()
        {
            return $"{DateTime.UtcNow.Ticks:D19}-{Guid.NewGuid()}";
        }
    }
}