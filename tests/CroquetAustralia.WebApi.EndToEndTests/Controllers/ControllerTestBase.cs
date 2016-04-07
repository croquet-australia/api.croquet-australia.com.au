using System;
using CroquetAustralia.WebApi.CommonTestHelpers;

namespace CroquetAustralia.WebApi.EndToEndTests.Controllers
{
    public abstract class ControllerTestBase
    {
        private readonly Lazy<WebApiClient> _lazyWebApiClient;
        private readonly Lazy<WebApiServer> _webApiServer;

        protected ControllerTestBase()
        {
            _lazyWebApiClient = new Lazy<WebApiClient>(CreateWebApiClient);
            _webApiServer = new Lazy<WebApiServer>(CreateWebApiServer);
        }

        protected WebApiClient WebApi => _lazyWebApiClient.Value;

        private WebApiServer CreateWebApiServer()
        {
            return WebApiServer.GetOrStart("http://localhost:5000");
        }

        private WebApiClient CreateWebApiClient()
        {
            return new WebApiClient(_webApiServer.Value.Url);
        }
    }
}