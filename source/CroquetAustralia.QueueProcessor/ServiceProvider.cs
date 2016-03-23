using System;
using CroquetAustralia.Domain.Features.TournamentEntry.Commands;
using CroquetAustralia.Domain.Services;
using CroquetAustralia.Domain.Services.Queues;
using CroquetAustralia.Domain.Services.Repositories;
using CroquetAustralia.QueueProcessor.Email;
using CroquetAustralia.QueueProcessor.Helpers;
using Ninject;

namespace CroquetAustralia.QueueProcessor
{
    public class ServiceProvider : IServiceProvider
    {
        private readonly IKernel _kernel;

        public ServiceProvider()
        {
            _kernel = new StandardKernel();

            _kernel.Bind<IAzureStorageConnectionString>().To<AzureStorageConnectionString>();
            _kernel.Bind<IEmailGenerator>().To<EmailGenerator>();
            _kernel.Bind<IEmailService>().To<EmailService>();
            _kernel.Bind<IEventsQueue>().To<EventsQueue>();
            _kernel.Bind<IEnveloper>().To<Enveloper>();
            _kernel.Bind<ITournamentsRepository>().To<TournamentsRepository>();
        }

        public object GetService(Type serviceType)
        {
            return _kernel.GetService(serviceType);
        }
    }
}