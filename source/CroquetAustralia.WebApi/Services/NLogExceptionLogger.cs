using System.Net.Http;
using System.Text;
using System.Web.Http.ExceptionHandling;
using Anotar.NLog;

namespace CroquetAustralia.WebApi.Services
{
    public class NLogExceptionLogger : ExceptionLogger
    {
        public override void Log(ExceptionLoggerContext context)
        {
            LogTo.ErrorException(() => RequestToString(context.Request), context.Exception);
        }

        private static string RequestToString(HttpRequestMessage request)
        {
            var message = new StringBuilder();
            if (request.Method != null)
                message.Append(request.Method);

            if (request.RequestUri != null)
                message.Append(" ").Append(request.RequestUri);

            return message.ToString();
        }
    }
}