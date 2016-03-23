using System;
using CroquetAustralia.Domain.Features.TournamentEntry.Commands;
using CroquetAustralia.Domain.Services.Repositories;
using CroquetAustralia.QueueProcessor.Email;
using Ninject;
using OpenMagic;

namespace CroquetAustralia.QueueProcessor.Specifications.TestHelpers
{
    public class ServiceProvider
    {
        public static IServiceProvider Instance = CreateServiceProvider();

        private static IServiceProvider CreateServiceProvider()
        {
            var kernel = new StandardKernel();

            kernel.Bind<IDummy>().To<DummyFactory>();
            kernel.Bind<IEmailGenerator>().To<EmailGenerator>();
            kernel.Bind<ITournamentsRepository>().To<TournamentsRepository>();

            return kernel;
        }
    }
}