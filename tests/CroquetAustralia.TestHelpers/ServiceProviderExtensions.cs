using System;

namespace CroquetAustralia.TestHelpers
{
    public static class ServiceProviderExtensions
    {
        public static TService Get<TService>(this IServiceProvider serviceProvider)
        {
            var service = serviceProvider.GetService(typeof(TService));

            if (service == null)
            {
                throw new NotImplementedException($"Service '{typeof(TService)}' has not been registered with service provider '{serviceProvider.GetType()}'.");
            }

            return (TService)service;
        }
    }
}