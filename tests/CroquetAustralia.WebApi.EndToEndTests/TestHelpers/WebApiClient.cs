using System;
using System.Net.Http;
using OpenMagic.Extensions;

namespace CroquetAustralia.WebApi.EndToEndTests.TestHelpers
{
    public class WebApiClient
    {
        private readonly Uri _uri;

        public WebApiClient()
        {
            _uri = new Uri("https://localhost:44300/");

            if (!_uri.IsResponding())
            {
                throw new Exception("Cannot run CroquetAustralia.WebApi.EndToEndTests until CroquetAustralia.WebApi is running.");
            }
        }

        public HttpResponseMessage Post(string resource, object command)
        {
            using (var client = NewHttpClient())
            {
                return client.PostAsJsonAsync(resource, command).Result;
            }
        }

        private HttpClient NewHttpClient()
        {
            return new HttpClient
            {
                BaseAddress = _uri
            };
        }
    }
}