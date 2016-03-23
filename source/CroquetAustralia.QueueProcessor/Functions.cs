using System.IO;
using System.Threading.Tasks;
using CroquetAustralia.Domain.Services.Queues;
using CroquetAustralia.QueueProcessor.Processors;
using Microsoft.Azure.WebJobs;

namespace CroquetAustralia.QueueProcessor
{
    public class Functions
    {
        private readonly EventsQueueProcessor _eventsQueueProcessor;
        private readonly SendEntrySubmittedEmailQueueProcessor _sendEntrySubmittedEmailQueueProcessor;

        public Functions(EventsQueueProcessor eventsQueueProcessor, SendEntrySubmittedEmailQueueProcessor sendEntrySubmittedEmailQueueProcessor)
        {
            _eventsQueueProcessor = eventsQueueProcessor;
            _sendEntrySubmittedEmailQueueProcessor = sendEntrySubmittedEmailQueueProcessor;
        }

        public async Task EventsQueueHandler([QueueTrigger(EventsQueue.QueueName)] string message, TextWriter logger)
        {
            await _eventsQueueProcessor.ProcessEventAsync(message, logger);
        }

        public async Task SendEntrySubmittedEmailQueueHandler([QueueTrigger(SendEntrySubmittedEmailQueue.QueueName)] string message, TextWriter logger)
        {
            await _sendEntrySubmittedEmailQueueProcessor.ProcessEventAsync(message, logger);
        }
    }
}