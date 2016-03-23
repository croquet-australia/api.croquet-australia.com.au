using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Anotar.NLog;
using CroquetAustralia.Domain.Features.TournamentEntry.Events;
using CroquetAustralia.Domain.Services.Queues;
using CroquetAustralia.Domain.Services.Serializers;
using CroquetAustralia.QueueProcessor.Email;

namespace CroquetAustralia.QueueProcessor.Processors
{
    public class SendEntrySubmittedEmailQueueProcessor
    {
        private readonly IEmailGenerator _emailGenerator;
        private readonly IEmailService _emailService;
        private readonly IEventsQueue _eventsQueue;

        private readonly QueueMessageSerializer _queueMessageSerializer;

        public SendEntrySubmittedEmailQueueProcessor(QueueMessageSerializer queueMessageSerializer, IEventsQueue eventsQueue, IEmailService emailService, IEmailGenerator emailGenerator)
        {
            _queueMessageSerializer = queueMessageSerializer;
            _eventsQueue = eventsQueue;
            _emailService = emailService;
            _emailGenerator = emailGenerator;
        }

        public async Task ProcessEventAsync(string message, TextWriter logger)
        {
            var @event = _queueMessageSerializer.Deserialize<EntrySubmitted>(message);

            await ProcessEventAsync(@event, @event.GetType(), logger);
        }

        public async Task ProcessEventAsync(EntrySubmitted @event, Type eventType, TextWriter logger)
        {
            LogInfo(logger, $"Processing event '{eventType.FullName}' from queue '{SendEntrySubmittedEmailQueue.QueueName}'...");

            var sentResults = (await SendEmailAsync(@event)).ToArray();

            LogInfo(logger, $"Sent {sentResults.Length:N0} emails.");

            foreach (var sentResult in sentResults)
            {
                LogInfo(logger, $"Adding event '{typeof(SentEntrySubmittedEmail).FullName}' to queue 'Events'...");

                var emailId = sentResult.Key;
                var emailMessage = sentResult.Value;
                var sentEmailEvent = new SentEntrySubmittedEmail(emailId, emailMessage, @event);

                await _eventsQueue.AddMessageAsync(sentEmailEvent);

                LogInfo(logger, $"Successfully added event '{typeof(SentEntrySubmittedEmail).FullName}' to queue 'Events'.");
            }

            LogInfo(logger, $"Successfully processed event '{eventType.FullName}' from queue '{SendEntrySubmittedEmailQueue.QueueName}'.");
        }

        private static void LogInfo(TextWriter logger, string message)
        {
            LogTo.Info(message);
            logger.WriteLineAsync(message).Wait();
        }

        private async Task<IEnumerable<KeyValuePair<string, EmailMessage>>> SendEmailAsync(EntrySubmitted @event)
        {
            var emailMessages = await _emailGenerator.GenerateAsync(@event);
            var sendingTasks = emailMessages.Select(async emailMessage => new KeyValuePair<string, EmailMessage>(await _emailService.SendAsync(emailMessage), emailMessage));
            var sentResults = await Task.WhenAll(sendingTasks);

            return sentResults;
        }
    }
}