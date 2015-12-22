using Microsoft.Azure.WebJobs;

namespace CroquetAustralia.QueueProcessor.Helpers
{
    internal class QueueNameResolver : INameResolver
    {
        private readonly string _prefix;

        public QueueNameResolver(QueueNameSettings queueNameSettings)
        {
            _prefix = queueNameSettings.Prefix;
        }

        public string Resolve(string key)
        {
            return $"{_prefix}{key}";
        }
    }
}