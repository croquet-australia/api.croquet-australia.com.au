using Newtonsoft.Json.Linq;

namespace CroquetAustralia.Domain.Services.Serializers
{
    public class EventsQueueEnveloper : IEnveloper
    {
        public JToken GetMessage(JObject envelope)
        {
            return envelope["Event"];
        }

        public JToken GetType(JObject envelope)
        {
            return envelope["EventType"];
        }

        public object CreateEnvelope(string type, object message)
        {
            return new {EventType = type, Event = message};
        }
    }
}