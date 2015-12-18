using System.Threading.Tasks;
using CroquetAustralia.Domain.Core;

namespace CroquetAustralia.Domain.Services
{
    public interface IEventQueue
    {
        Task AddEventAsync(IEvent @event);
    }
}