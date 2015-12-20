using CroquetAustralia.Domain.Services;

namespace CroquetAustralia.WebApi.Settings
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