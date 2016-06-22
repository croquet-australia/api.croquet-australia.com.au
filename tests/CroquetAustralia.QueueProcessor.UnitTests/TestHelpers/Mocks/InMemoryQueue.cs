using System.Collections.Generic;
using System.Threading.Tasks;
using CroquetAustralia.Domain.Services.Queues;

namespace CroquetAustralia.QueueProcessor.UnitTests.TestHelpers.Mocks
{
    public class InMemoryQueue : IQueueBase
    {
        // ReSharper disable once CollectionNeverQueried.Local
        private readonly List<object> _messages = new List<object>();

        public Task AddMessageAsync(object message)
        {
            return Task.Run(() => _messages.Add(message));
        }
    }
}