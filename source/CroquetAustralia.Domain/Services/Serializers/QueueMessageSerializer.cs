using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CroquetAustralia.Domain.Core;
using CroquetAustralia.Domain.Services.Queues;
using CroquetAustralia.Library.Extensions;
using Newtonsoft.Json.Linq;

namespace CroquetAustralia.Domain.Services.Serializers
{
    public class QueueMessageSerializer
    {
        private static readonly Dictionary<string, Type> DomainTypes;
        private static readonly string DomainAssemblyName;
        private readonly IEnveloper _enveloper;

        static QueueMessageSerializer()
        {
            var domainAssembly = Assembly.GetAssembly(typeof(EventsQueue));
            var exportedTypes = domainAssembly.ExportedTypes;

            DomainAssemblyName = domainAssembly.FullName;
            DomainTypes = exportedTypes.ToDictionary(t => t.FullName, t => t);
        }

        public QueueMessageSerializer()
            : this(new Enveloper())
        {
        }

        protected QueueMessageSerializer(IEnveloper enveloper)
        {
            _enveloper = enveloper;
        }

        public string Serialize(object message)
        {
            var envelope = _enveloper.CreateEnvelope(message.GetType().FullName, message);
            var json = envelope.ToJson();

            return json;
        }

        public object Deserialize(string message)
        {
            var envelope = message.ToObject<JObject>();

            var typeToken = _enveloper.GetType(envelope);
            var typeName = typeToken.ToObject<string>();
            var type = GetEventType(typeName);

            var messageToken = _enveloper.GetMessage(envelope);
            var messageObject = messageToken.ToObject(type);

            return messageObject;
        }

        public TEvent Deserialize<TEvent>(string message) where TEvent : IEvent
        {
            var methodObject = Deserialize(message);
            var @event = (TEvent)methodObject;

            return @event;
        }

        private static Type GetEventType(string eventType)
        {
            Type type;

            if (DomainTypes.TryGetValue(eventType, out type))
            {
                return type;
            }

            throw new Exception(
                $"Message cannot be deserialized because type '{eventType}' cannot be found in domain assembly '{DomainAssemblyName}'.");
        }
    }
}