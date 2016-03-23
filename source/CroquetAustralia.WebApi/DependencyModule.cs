using CroquetAustralia.Domain.Features.TournamentEntry.Commands;
using CroquetAustralia.Domain.Services;
using CroquetAustralia.Domain.Services.Queues;
using CroquetAustralia.Domain.Services.Repositories;
using CroquetAustralia.Domain.Settings;
using CroquetAustralia.WebApi.Settings;
using Ninject.Modules;
using Ninject.Web.Common;

namespace CroquetAustralia.WebApi
{
    public class DependencyModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IAzureStorageConnectionString>().To<AzureStorageConnectionString>();
            Bind<IConnectionStringSettings>().To<ConnectionStringSettings>();
            Bind<IEventsQueue>().To<EventsQueue>().InSingletonScope();
            Bind<ITournamentsRepository>().To<TournamentsRepository>().InRequestScope();
        }
    }
}