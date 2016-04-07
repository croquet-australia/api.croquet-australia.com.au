using System.Web.Http;
using CroquetAustralia.WebApi;
using CroquetAustralia.WebApi.Settings;
using Microsoft.Owin;
using Ninject;
using Ninject.Web.Common.OwinHost;
using Ninject.Web.WebApi.OwinHost;
using Owin;

[assembly: OwinStartup(typeof(Startup))]

namespace CroquetAustralia.WebApi
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=316888
            var config = new HttpConfiguration();
            var kernel = IoC.Configure();

            WebApiConfig.Configure(config, kernel.Get<WebApiSettings>(), kernel.Get<WebAppSettings>());

            app.UseNinjectMiddleware(() => kernel)
                .UseNinjectWebApi(config);
        }
    }
}