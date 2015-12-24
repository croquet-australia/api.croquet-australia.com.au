using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Anotar.NLog;
using CroquetAustralia.Domain.Features.TournamentEntry.Events;
using CroquetAustralia.Domain.Services.Queues;
using CroquetAustralia.Domain.Services.Serializers;
using CroquetAustralia.Library.Extensions;
using CroquetAustralia.QueueProcessor.Email;
using CroquetAustralia.QueueProcessor.Helpers;

namespace CroquetAustralia.QueueProcessor.Processors
{
    public class SendEntrySubmittedEmailQueueProcessor
    {
        private readonly IEventsQueue _eventsQueue;

        private readonly QueueMessageSerializer _queueMessageSerializer;
        private readonly IEmailService _emailService;

        // todo: proper dependency injection?
        public SendEntrySubmittedEmailQueueProcessor() : this(
            new QueueMessageSerializer(),
            new EventsQueue(new AzureStorageConnectionString()),
            new EmailService())
        {
        }

        public SendEntrySubmittedEmailQueueProcessor(
            QueueMessageSerializer queueMessageSerializer,
            IEventsQueue eventsQueue,
            IEmailService emailService)
        {
            _queueMessageSerializer = queueMessageSerializer;
            _eventsQueue = eventsQueue;
            _emailService = emailService;
        }

        public async Task ProcessEventAsync(string message, TextWriter logger)
        {
            var @event = _queueMessageSerializer.Deserialize<EntrySubmitted>(message);

            await ProcessEventAsync(@event, @event.GetType(), logger);
        }

        public async Task ProcessEventAsync(EntrySubmitted @event, Type eventType, TextWriter logger)
        {
            LogTo.Info($"Processing '{eventType.FullName}' event.");

            await logger.WriteLineAsync($"Received {eventType.FullName} from {SendEntrySubmittedEmailQueue.QueueName}...");

            var emailId = await SendEmailAsync(@event, logger);

            await logger.WriteLineAsync($"Adding {typeof (SentEntrySubmittedEmail).FullName} to events queue...");
            var sentEntrySubmittedEmailEvent = new SentEntrySubmittedEmail(@event, emailId);
            await _eventsQueue.AddMessageAsync(sentEntrySubmittedEmailEvent);

            await logger.WriteLineAsync($"Successfully processed {eventType.FullName} from {SendEntrySubmittedEmailQueue.QueueName}...");
        }

        private async Task<string> SendEmailAsync(EntrySubmitted @event, TextWriter logger)
        {
            var templateNamespace = $"CroquetAustralia.QueueProcessor.Email.Templates.PayBy{@event.PaymentMethod}";
            var templateName = GetTemplateName(@event);

            await logger.WriteLineAsync($"Using email template {templateName}.");

            var template = GetTemplate($"{templateNamespace}.{templateName}.txt");
            var emailMessage = new EmailMessage(template, @event);
            var emailId = await _emailService.SendAsync(emailMessage);

            return emailId;
        }

        private string GetTemplate(string resourceName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var template = assembly.GetResourceText(resourceName);

            return template;
        }

        // ReSharper disable once SuggestBaseTypeForParameter
        private string GetTemplateName(EntrySubmitted @event)
        {
            if (@event.EventId.HasValue)
            {
                return @event.Functions.Any() ? "Event and Functions" : "Event Only";
            }

            if (@event.Functions.Any())
            {
                return "Functions Only";
            }

            throw new NotSupportedException("Received an EntrySumbitted event where we don't know what email template to use.")
            {
                Data = {{"Event", @event}}
            };
        }
    }
}