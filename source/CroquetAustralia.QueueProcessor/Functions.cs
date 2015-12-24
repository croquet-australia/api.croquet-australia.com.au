using System.IO;
using System.Threading.Tasks;
using Anotar.NLog;
using CroquetAustralia.Domain.Services.Queues;
using CroquetAustralia.QueueProcessor.Processors;
using Microsoft.Azure.WebJobs;

namespace CroquetAustralia.QueueProcessor
{
    public class Functions
    {
        private static readonly EventsQueueProcessor EventsQueueProcessor;
        private static readonly SendEntrySubmittedEmailQueueProcessor SendEntrySubmittedEmailQueueProcessor;

        static Functions()
        {
            EventsQueueProcessor = new EventsQueueProcessor();
            SendEntrySubmittedEmailQueueProcessor = new SendEntrySubmittedEmailQueueProcessor();
        }

        public async Task EventsQueueHandler([QueueTrigger(EventsQueue.QueueName)] string message, TextWriter logger)
        {
            LogTo.Info($"Received message from '{EventsQueue.QueueName}' queue.");
            await EventsQueueProcessor.ProcessEventAsync(message, logger);
        }

        public static async Task SendEntrySubmittedEmailQueueHandler([QueueTrigger(SendEntrySubmittedEmailQueue.QueueName)] string message, TextWriter logger)
        {
            LogTo.Info($"Received message from '{SendEntrySubmittedEmailQueue.QueueName}' queue.");
            await SendEntrySubmittedEmailQueueProcessor.ProcessEventAsync(message, logger);
        }
    }
}
