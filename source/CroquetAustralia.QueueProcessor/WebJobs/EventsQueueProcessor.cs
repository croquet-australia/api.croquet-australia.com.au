using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using CroquetAustralia.Domain.Core;
using CroquetAustralia.Domain.Features.TournamentEntry.Events;
using CroquetAustralia.Domain.Services.Queues;
using CroquetAustralia.Domain.Services.Repositories;
using CroquetAustralia.Domain.Services.Serializers;
using CroquetAustralia.QueueProcessor.Helpers;
using Microsoft.Azure.WebJobs;

namespace CroquetAustralia.QueueProcessor.WebJobs
{
    public class EventsQueueProcessor
    {
        private const string QueueName = EventsQueue.QueueName;
        private const string QueueWildcardName = "%" + QueueName + "%";

        private readonly Dictionary<Type, Func<IEvent, Type, TextWriter, Task>> _eventProcessors;
        private readonly EventsQueueMessageSerializer _eventsQueueMessageSerializer;
        private readonly IEventsRepository _eventsRepository;
        private readonly ISendEntrySubmittedEmailQueue _sendEntrySubmittedEmailQueue;

        // todo: proper dependency injection?
        public EventsQueueProcessor() : this(
            new EventsQueueMessageSerializer(),
            new SendEntrySubmittedEmailQueue(new AzureStorageConnectionString()),
            new EventsRepository(new AzureStorageConnectionString()))
        {
        }

        private EventsQueueProcessor(
            EventsQueueMessageSerializer eventsQueueMessageSerializer,
            ISendEntrySubmittedEmailQueue sendEntrySubmittedEmailQueue,
            IEventsRepository eventsRepository)
        {
            _eventsQueueMessageSerializer = eventsQueueMessageSerializer;
            _sendEntrySubmittedEmailQueue = sendEntrySubmittedEmailQueue;
            _eventsRepository = eventsRepository;

            _eventProcessors = new Dictionary<Type, Func<IEvent, Type, TextWriter, Task>>
            {
                {
                    typeof (EntrySubmitted), EntrySubmittedProcessorAsync
                }
            };
        }

        public async Task EventsQueueReaderAsync([QueueTrigger(QueueWildcardName)] string message, TextWriter logger)
        {
            var @event = (IEvent) _eventsQueueMessageSerializer.Deserialize(message);

            await ProcessEventAsync(@event, @event.GetType(), logger);
        }

        public async Task ProcessEventAsync(IEvent @event, Type eventType, TextWriter logger)
        {
            await logger.WriteLineAsync($"Received {eventType} from {QueueName}...");
            var eventProcessor = GetEventProcessor(eventType);

            await logger.WriteLineAsync($"Processing {eventType} from {QueueName}...");
            await eventProcessor(@event, eventType, logger);

            await logger.WriteLineAsync($"Adding {eventType} to Events table...");
            await _eventsRepository.AddAsync(@event);

            await logger.WriteLineAsync($"Successfully processed {eventType} from {QueueName}...");
        }

        private Func<IEvent, Type, TextWriter, Task> GetEventProcessor(Type eventType)
        {
            Func<IEvent, Type, TextWriter, Task> eventProcessor;

            if (!_eventProcessors.TryGetValue(eventType, out eventProcessor))
            {
                throw new NotSupportedException($"'{0}' events are not supported.");
            }

            return eventProcessor;
        }

        private async Task EntrySubmittedProcessorAsync(IEvent @event, Type eventType, TextWriter logger)
        {
            await _sendEntrySubmittedEmailQueue.AddMessageAsync(@event);
        }
    }
}