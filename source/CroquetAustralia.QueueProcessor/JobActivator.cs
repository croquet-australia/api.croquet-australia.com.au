using System;
using Microsoft.Azure.WebJobs.Host;

namespace CroquetAustralia.QueueProcessor
{
    public class JobActivator : IJobActivator
    {
        private readonly ServiceProvider _serviceProvider;

        public JobActivator(ServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public T CreateInstance<T>()
        {
            return (T)_serviceProvider.GetService(typeof(T));
        }
    }
}