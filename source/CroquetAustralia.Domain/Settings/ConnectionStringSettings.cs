using CroquetAustralia.Library.Settings;

namespace CroquetAustralia.Domain.Settings
{
    public class ConnectionStringSettings : BaseAppSettings, IConnectionStringSettings
    {
        public ConnectionStringSettings() : base("ConnectionString")
        {
            AzureStorage = Get("AzureStorage");
        }

        public string AzureStorage { get; }
    }
}