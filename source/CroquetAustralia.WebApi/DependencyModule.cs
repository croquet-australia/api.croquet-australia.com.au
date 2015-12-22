using CroquetAustralia.Domain.Services;
using CroquetAustralia.Domain.Services.Queues;
using CroquetAustralia.Domain.Settings;
using CroquetAustralia.WebApi.Settings;
using Ninject.Modules;

namespace CroquetAustralia.WebApi
{
    public class DependencyModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IAzureStorageConnectionString>().To<AzureStorageConnectionString>();
            Bind<IConnectionStringSettings>().To<ConnectionStringSettings>();
            Bind<IEventsQueue>().To<EventsQueue>().InSingletonScope();
        }
    }
}