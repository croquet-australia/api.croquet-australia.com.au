using Anotar.NLog;
using CroquetAustralia.Domain.Services;
using Ninject.Modules;

namespace CroquetAustralia.WebApi
{
    public class DependencyModule : NinjectModule
    {
        public override void Load()
        {
            LogTo.Trace("Load() - in s");
            Bind<IEventQueue>().To<EventQueue>().InSingletonScope();
        }
    }
}