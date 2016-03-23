using System;

namespace CroquetAustralia.Library
{
    public static class ServiceFactory
    {
        private static IServiceProvider _serviceProvider;

        public static void SetServiceProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public static T Get<T>()
        {
            if (_serviceProvider == null)
            {
                throw new InvalidOperationException($"Call '{nameof(ServiceFactory)}.{nameof(SetServiceProvider)}({nameof(IServiceProvider)})' before calling '{nameof(ServiceFactory)}.{nameof(Get)}<{typeof(T)}>()'");
            }

            var service = _serviceProvider.GetService(typeof(T));

            if (service == null)
            {
                throw new InvalidOperationException($"Service '{typeof(T)}' must be registered with service provider first.");
            }

            return (T)service;
        }
    }
}