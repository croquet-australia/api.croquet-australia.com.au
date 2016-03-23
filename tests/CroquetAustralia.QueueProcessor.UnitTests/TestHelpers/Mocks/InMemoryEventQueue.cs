using CroquetAustralia.Domain.Services.Queues;

namespace CroquetAustralia.QueueProcessor.UnitTests.TestHelpers.Mocks
{
    public class InMemoryEventQueue : InMemoryQueue, IEventsQueue
    {
    }
}