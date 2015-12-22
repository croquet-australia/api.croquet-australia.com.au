using System.Threading.Tasks;
using CroquetAustralia.Domain.Core;
using CroquetAustralia.Domain.Services.Repositories.TableEntities;

namespace CroquetAustralia.Domain.Services.Repositories
{
    public class EventsRepository : RepositoryBase, IEventsRepository
    {
        public EventsRepository(IAzureStorageConnectionString azureStorageConnectionString)
            : base("Events", azureStorageConnectionString)
        {
        }

        public async Task AddAsync(IEvent @event)
        {
            var tableEntity = new EventsTableEntity(@event);
            await AddAsync(tableEntity);
        }
    }
}