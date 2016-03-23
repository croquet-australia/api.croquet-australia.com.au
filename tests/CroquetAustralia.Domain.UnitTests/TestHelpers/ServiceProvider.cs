using System;
using CroquetAustralia.Domain.Features.TournamentEntry.Commands;
using CroquetAustralia.Domain.Services.Repositories;
using Ninject;
using OpenMagic;

namespace CroquetAustralia.Domain.UnitTests.TestHelpers
{
    public class ServiceProvider
    {
        public static IServiceProvider Instance = CreateServiceProvider();

        private static IServiceProvider CreateServiceProvider()
        {
            var kernel = new StandardKernel();

            kernel.Bind<IDummy>().To<DummyFactory>();
            kernel.Bind<ITournamentsRepository>().To<TournamentsRepository>();

            return kernel;
        }
    }
}