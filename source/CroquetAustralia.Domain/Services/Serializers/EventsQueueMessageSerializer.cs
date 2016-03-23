namespace CroquetAustralia.Domain.Services.Serializers
{
    public class EventsQueueMessageSerializer : QueueMessageSerializer
    {
        public EventsQueueMessageSerializer() : base(new EventsQueueEnveloper())
        {
        }
    }
}