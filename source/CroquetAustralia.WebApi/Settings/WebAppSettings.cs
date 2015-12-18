namespace CroquetAustralia.WebApi.Settings
{
    public class WebAppSettings : BaseAppSettings
    {
        public readonly string BaseUri;

        public WebAppSettings() : base("WebApp")
        {
            BaseUri = Get("BaseUri").TrimEnd('/');
        }
    }
}