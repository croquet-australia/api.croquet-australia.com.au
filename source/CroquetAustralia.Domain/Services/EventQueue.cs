using System;
using System.Threading.Tasks;
using CroquetAustralia.Domain.Core;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;

namespace CroquetAustralia.Domain.Services
{
    public class EventQueue : IEventQueue
    {
        private readonly Lazy<CloudQueue> _lazyQueue;

        public EventQueue(CloudStorage cloudStorage)
        {
            _lazyQueue = new Lazy<CloudQueue>(cloudStorage.GetEventsQueue);
        }

        private CloudQueue Queue => _lazyQueue.Value;

        public async Task AddEventAsync(IEvent @event)
        {
            var content = new
            {
                EventType = @event.GetType().FullName,
                Event = @event
            };

            var json = JsonConvert.SerializeObject(content, Formatting.None);
            var message = new CloudQueueMessage(json);

            await Queue.AddMessageAsync(message);
        }
    }
}