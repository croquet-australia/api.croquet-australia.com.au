using CroquetAustralia.Domain.Features.TournamentEntry.Commands;
using CroquetAustralia.Domain.Services;
using CroquetAustralia.Domain.Services.Queues;
using CroquetAustralia.Domain.Services.Repositories;
using CroquetAustralia.Domain.Settings;
using CroquetAustralia.Library;
using CroquetAustralia.WebApi.Settings;
using Ninject;
using Ninject.Syntax;
using Ninject.Web.Common;

namespace CroquetAustralia.WebApi
{
    public class IoC
    {
        public static IKernel Configure()
        {
            var kernel = new StandardKernel();

            BindServices(kernel);
            ServiceFactory.SetServiceProvider(kernel);

            return kernel;
        }

        private static void BindServices(IBindingRoot kernel)
        {
            kernel.Bind<IAzureStorageConnectionString>().To<AzureStorageConnectionString>();
            kernel.Bind<IConnectionStringSettings>().To<ConnectionStringSettings>();
            kernel.Bind<IEventsQueue>().To<EventsQueue>().InSingletonScope();
            kernel.Bind<ITournamentsRepository>().To<TournamentsRepository>().InRequestScope();
        }
    }
}