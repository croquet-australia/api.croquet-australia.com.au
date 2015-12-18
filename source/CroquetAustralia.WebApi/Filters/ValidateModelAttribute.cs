using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Http.ModelBinding;
using Anotar.NLog;

namespace CroquetAustralia.WebApi.Filters
{
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (actionContext.ModelState.IsValid)
            {
                return;
            }
            LogTo.Debug(() => $"OnActionExecuting(HttpActionContext): {AsLines(actionContext.ModelState)}");

            actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.BadRequest, actionContext.ModelState);
        }

        private static string AsLines(ModelStateDictionary modelState)
        {
            return string.Join(Environment.NewLine, modelState.SelectMany(m => m.Value.Errors));
        }
    }
}