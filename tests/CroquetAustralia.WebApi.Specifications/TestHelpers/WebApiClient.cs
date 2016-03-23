using System;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenMagic.Extensions;

namespace CroquetAustralia.WebApi.Specifications.TestHelpers
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
                var response = client.PostAsJsonAsync(resource, command).Result;

                if (response.StatusCode != HttpStatusCode.InternalServerError)
                {
                    return response;
                }

                var content = response.Content.ReadAsAsync<JObject>().Result;
                var message = content["ExceptionMessage"];
                var stackTrace = content["StackTrace"];

                throw new Exception($"Post '{resource}' failed with {HttpStatusCode.InternalServerError}.\n\n{JsonConvert.SerializeObject(command, Formatting.Indented)}\n\n{message}\n\n{stackTrace}");
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