using System;
using System.Collections.Concurrent;
using Microsoft.Owin.Hosting;

namespace CroquetAustralia.WebApi.CommonTestHelpers
{
    public class WebApiServer
    {
        private static readonly ConcurrentDictionary<string, WebApiServer> Servers;
        private IDisposable _server;

        static WebApiServer()
        {
            Servers = new ConcurrentDictionary<string, WebApiServer>();
        }

        private WebApiServer(string url, bool startServer = false)
        {
            Url = url;

            if (startServer)
            {
                StartServer();
            }
        }

        public string Url { get; }

        private void StartServer()
        {
            if (IsRunning())
            {
                throw new InvalidOperationException("Server cannot be started multiple times.");
            }

            _server = WebApp.Start<Startup>(Url);
        }

        private bool IsRunning()
        {
            return _server != null;
        }

        public static WebApiServer GetOrStart(string url)
        {
            return Servers.GetOrAdd(url, u => new WebApiServer(u, true));
        }
    }
}