using System;

namespace CroquetAustralia.WebApi.Specifications.TestHelpers
{
    public class ServiceProvider
    {
        public static IServiceProvider Instance = CreateServiceProvider();

        private static IServiceProvider CreateServiceProvider()
        {
            return Domain.UnitTests.TestHelpers.ServiceProvider.Instance;
        }
    }
}