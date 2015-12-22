using System;
using Newtonsoft.Json.Linq;

namespace CroquetAustralia.Domain.Services
{
    public interface IEnveloper
    {
        JToken GetMessage(JObject envelope);
        JToken GetType(JObject envelope);

        object CreateEnvelope(string type, object message);
    }
}