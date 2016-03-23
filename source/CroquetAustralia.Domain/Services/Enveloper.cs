using Newtonsoft.Json.Linq;

namespace CroquetAustralia.Domain.Services
{
    public class Enveloper : IEnveloper
    {
        public JToken GetMessage(JObject envelope)
        {
            return envelope["message"];
        }

        public JToken GetType(JObject envelope)
        {
            return envelope["type"];
        }

        public object CreateEnvelope(string type, object message)
        {
            return new {type, message};
        }
    }
}