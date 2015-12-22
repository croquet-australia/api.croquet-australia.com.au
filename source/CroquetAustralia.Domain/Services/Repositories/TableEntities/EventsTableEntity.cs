using CroquetAustralia.Domain.Core;
using CroquetAustralia.Library.Extensions;
using Microsoft.WindowsAzure.Storage.Table;

namespace CroquetAustralia.Domain.Services.Repositories.TableEntities
{
    public class EventsTableEntity : TableEntity
    {
        public EventsTableEntity(IEvent @event) 
            : base(@event.EntityId.ToString(), RepositoryBase.CreateTimestampRowKey())
        {
            EventType = @event.GetType().FullName;
            Event = @event.ToJson();
        }

        public string EventType { get; set; }
        public string Event { get; set; }
    }
}