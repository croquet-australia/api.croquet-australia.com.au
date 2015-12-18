using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace CroquetAustralia.WebApi.Filters
{
    public class RequireHttpsAttribute : AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (actionContext.Request.RequestUri.Scheme != Uri.UriSchemeHttps && !IsForwardedSsl(actionContext))
            {
                actionContext.Response = new HttpResponseMessage(HttpStatusCode.Forbidden)
                {
                    ReasonPhrase = "HTTPS Required"
                };
            }
            else
            {
                base.OnAuthorization(actionContext);
            }
        }

        private static bool IsForwardedSsl(HttpActionContext actionContext)
        {
            var xForwardedProto = actionContext.Request.Headers.FirstOrDefault(x => x.Key == "X-Forwarded-Proto");
            var forwardedSsl = xForwardedProto.Value?.Any(x => x.Equals("https", StringComparison.InvariantCultureIgnoreCase));

            return forwardedSsl.GetValueOrDefault();
        }
    }
}