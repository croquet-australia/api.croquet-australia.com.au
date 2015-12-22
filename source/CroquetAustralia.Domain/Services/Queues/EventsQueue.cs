using CroquetAustralia.Domain.Services.Serializers;

namespace CroquetAustralia.Domain.Services.Queues
{
    public class EventsQueue : QueueBase, IEventsQueue
    {
        public const string QueueName = "events";

        public EventsQueue(IAzureStorageConnectionString connectionString) 
            : base(QueueName, connectionString, new EventsQueueMessageSerializer())
        {
        }
    }
}