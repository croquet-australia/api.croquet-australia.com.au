using System;
using System.Threading.Tasks;
using Anotar.NLog;
using CroquetAustralia.Domain.Services.Serializers;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;

namespace CroquetAustralia.Domain.Services.Queues
{
    public abstract class QueueBase : IQueueBase
    {
        private readonly Lazy<CloudQueue> _lazyQueue;
        private readonly string _queueName;
        private readonly QueueMessageSerializer _serializer;

        protected QueueBase(string queueName, IAzureStorageConnectionString connectionString)
            : this(queueName, connectionString, new QueueMessageSerializer())
        {
        }

        protected QueueBase(string queueName, IAzureStorageConnectionString connectionString, QueueMessageSerializer serializer)
        {
            _queueName = queueName;
            _serializer = serializer;
            _lazyQueue = new Lazy<CloudQueue>(() => GetQueue(queueName, connectionString.Value));
        }

        private CloudQueue CloudQueue => _lazyQueue.Value;

        public async Task AddMessageAsync(object @event)
        {
            LogTo.Info($"Adding '{@event.GetType().FullName}' to '{_queueName}' queue.");

            var content = _serializer.Serialize(@event);
            var message = new CloudQueueMessage(content);

            await CloudQueue.AddMessageAsync(message);
        }

        private static CloudQueue GetQueue(string queueName, string connectionString)
        {
            var storageAccount = CloudStorageAccount.Parse(connectionString);
            var queueClient = storageAccount.CreateCloudQueueClient();

            var queue = queueClient.GetQueueReference(queueName);
            queue.CreateIfNotExists();

            return queue;
        }
    }
}