using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Filters;
using Anotar.NLog;
using CroquetAustralia.Domain.Core;
using CroquetAustralia.WebApi.Filters;
using CroquetAustralia.WebApi.Settings;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NodaTime;

namespace CroquetAustralia.WebApi
{
    public static class WebApiConfig
    {
        public static void Configure(HttpConfiguration config, WebApiSettings webApiSettings, WebAppSettings webAppSettings)
        {
            config.MapHttpAttributeRoutes();

            ConfigureCors(config, webAppSettings);
            ConfigureFormatters(config.Formatters);
            ConfigureFilters(config.Filters, webApiSettings);
        }

        private static void ConfigureCors(HttpConfiguration config, WebAppSettings webAppSettings)
        {
            var cors = new EnableCorsAttribute(webAppSettings.BaseUri, "*", "*");
            config.EnableCors(cors);

            LogTo.Info($"Enable CORS with origins '{string.Join(",", cors.Origins)}'.");
        }

        private static void ConfigureFormatters(MediaTypeFormatterCollection formatters)
        {
            formatters.Clear();
            formatters.Add(new JsonMediaTypeFormatter());

            var serializerSettings = formatters.JsonFormatter.SerializerSettings;

            serializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            serializerSettings.Converters.Add(new ZonedDateTimeJsonConverter(DateTimeZoneProviders.Tzdb));

            // todo: is this needed? 
            serializerSettings.DateParseHandling = DateParseHandling.None;
        }

        private static void ConfigureFilters(HttpFilterCollection filters, WebApiSettings webApiSettings)
        {
            if (!webApiSettings.RunningTests)
            {
                filters.Add(new RequireHttpsAttribute());
            }

            filters.Add(new EntityNotFoundFilterAttribute());
            filters.Add(new ValidateModelAttribute());
        }
    }
}