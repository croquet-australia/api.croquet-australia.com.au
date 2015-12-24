using CroquetAustralia.Domain.Services.Queues;

namespace CroquetAustralia.QueueProcessor.Tests.TestHelpers.Mocks
{
    public class InMemoryEventQueue : InMemoryQueue, IEventsQueue
    {
    }
}