using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;

namespace CroquetAustralia.Domain.Services
{
    public class CloudStorage
    {
        public static CloudQueue GetEventsQueue()
        {
            var storageAccount = CloudStorageAccount.Parse("UseDevelopmentStorage=true;");
            var queueClient = storageAccount.CreateCloudQueueClient();

            var queue = queueClient.GetQueueReference("events");
            queue.CreateIfNotExists();

            return queue;
        }
    }
}
