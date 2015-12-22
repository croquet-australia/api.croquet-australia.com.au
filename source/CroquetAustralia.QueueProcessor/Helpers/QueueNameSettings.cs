using CroquetAustralia.Library.Settings;
using NullGuard;

namespace CroquetAustralia.QueueProcessor.Helpers
{
    internal class QueueNameSettings: BaseAppSettings
    {
        public QueueNameSettings() : base("QueueName")
        {
            Prefix = Get("Prefix", true);
        }
        
        public string Prefix { [return: AllowNull] get; }
    }
}