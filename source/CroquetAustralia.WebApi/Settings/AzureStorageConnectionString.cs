using CroquetAustralia.Domain.Services;
using CroquetAustralia.Domain.Settings;

namespace CroquetAustralia.WebApi.Settings
{
    public class AzureStorageConnectionString : IAzureStorageConnectionString
    {
        public AzureStorageConnectionString()
        {
            Value = new ConnectionStringSettings().AzureStorage;
        }

        public string Value { get; }
    }
}