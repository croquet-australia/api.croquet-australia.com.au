using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;

namespace CroquetAustralia.Domain.Services
{
    public class CloudStorage
    {
        private readonly IConnectionStringSettings _connectionStringSettings;

        public CloudStorage(IConnectionStringSettings connectionStringSettings)
        {
            _connectionStringSettings = connectionStringSettings;
        }

        public CloudQueue GetEventsQueue()
        {
            var storageAccount = CloudStorageAccount.Parse(_connectionStringSettings.AzureStorage);
            var queueClient = storageAccount.CreateCloudQueueClient();

            var queue = queueClient.GetQueueReference("events");
            queue.CreateIfNotExists();

            return queue;
        }
    }
}
