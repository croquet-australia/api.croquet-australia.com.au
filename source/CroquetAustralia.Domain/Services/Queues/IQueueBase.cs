using System.Threading.Tasks;

namespace CroquetAustralia.Domain.Services.Queues
{
    public interface IQueueBase
    {
        Task AddMessageAsync(object message);
    }
}