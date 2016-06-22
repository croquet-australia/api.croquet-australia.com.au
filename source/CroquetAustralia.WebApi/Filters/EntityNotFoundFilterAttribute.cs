using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;
using Anotar.NLog;
using CroquetAustralia.Domain.Exceptions;

namespace CroquetAustralia.WebApi.Filters
{
    public class EntityNotFoundFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            base.OnException(context);

            // ReSharper disable once InvertIf
            if (context.Exception is EntityNotFoundException)
            {
                LogTo.DebugException("Entity not found.", context.Exception);
                context.Response = new HttpResponseMessage(HttpStatusCode.NotFound);
            }
        }
    }
}