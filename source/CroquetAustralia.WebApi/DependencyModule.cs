using CroquetAustralia.Domain.Services;
using CroquetAustralia.WebApi.Settings;
using Ninject.Modules;

namespace CroquetAustralia.WebApi
{
    public class DependencyModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IConnectionStringSettings>().To<ConnectionStringSettings>();
            Bind<IEventQueue>().To<EventQueue>().InSingletonScope();
        }
    }
}