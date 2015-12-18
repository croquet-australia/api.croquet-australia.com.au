using System.Threading.Tasks;
using Anotar.NLog;
using CroquetAustralia.Domain.Core;

namespace CroquetAustralia.Domain.Services
{
    public class EventQueue : IEventQueue
    {
        public Task AddEventAsync(IEvent @event)
        {
            LogTo.Warn($"todo: AddEventAsync(IEvent {@event.GetType()}");
            return Task.FromResult(0);
        }
    }
}
