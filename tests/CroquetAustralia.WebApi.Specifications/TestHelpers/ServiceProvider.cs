using System;
using Ninject;

namespace CroquetAustralia.WebApi.Specifications.TestHelpers
{
    public class ServiceProvider : IServiceProvider
    {
        public static IServiceProvider Instance = new ServiceProvider();

        public object GetService(Type serviceType)
        {
            object service;

            if (TryGetService(serviceType, CommonTestHelpers.ServiceProvider.Instance, out service))
            {
                return service;
            }

            if (TryGetService(serviceType, Domain.UnitTests.TestHelpers.ServiceProvider.Instance, out service))
            {
                return service;
            }

            return null;
        }

        private static bool TryGetService(Type serviceType, IServiceProvider instance, out object service)
        {
            try
            {
                service = instance.GetService(serviceType);
            }
            catch (ActivationException)
            {
                service = null;
            }

            return service != null;
        }
    }
}