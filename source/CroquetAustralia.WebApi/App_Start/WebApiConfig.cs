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
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            ConfigureCors(config);
            ConfigureFormatters(config.Formatters);
            ConfigureFilters(config.Filters);
        }

        private static void ConfigureCors(HttpConfiguration config)
        {
            var cors = new EnableCorsAttribute(new WebAppSettings().BaseUri, "*", "*");
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

        private static void ConfigureFilters(HttpFilterCollection filters)
        {
            filters.Add(new RequireHttpsAttribute());
            filters.Add(new ValidateModelAttribute());
        }
    }
}