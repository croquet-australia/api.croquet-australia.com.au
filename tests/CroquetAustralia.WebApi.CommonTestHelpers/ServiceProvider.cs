using System;
using Ninject;

namespace CroquetAustralia.WebApi.CommonTestHelpers
{
    public class ServiceProvider : IServiceProvider
    {
        public static IServiceProvider Instance = new ServiceProvider();
        private readonly IServiceProvider _serviceProvider;

        public ServiceProvider()
        {
            var kernel = new StandardKernel();

            kernel.Bind<WebApiServer>().ToMethod(x => WebApiServer.GetOrStart("http://localhost:4430"));

            _serviceProvider = kernel;
        }

        public object GetService(Type serviceType)
        {
            return _serviceProvider.GetService(serviceType);
        }
    }
}