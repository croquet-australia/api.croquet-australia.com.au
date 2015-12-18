using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Filters;
using CroquetAustralia.WebApi.Filters;

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
            var cors = new EnableCorsAttribute("https://localhost:44302", "*", "*");
            config.EnableCors(cors);
        }

        private static void ConfigureFormatters(MediaTypeFormatterCollection formatters)
        {
            formatters.Clear();
            formatters.Add(new JsonMediaTypeFormatter());
        }

        private static void ConfigureFilters(HttpFilterCollection filters)
        {
            filters.Add(new RequireHttpsAttribute());
        }
    }
}